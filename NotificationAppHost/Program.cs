var builder = DistributedApplication.CreateBuilder(args);

var userNameDefault = builder.AddParameter("name", secret: true);
var passwordDefault = builder.AddParameter("passwd", secret: true);

var rabbitMq = builder.AddRabbitMQ("rabbitMq", userNameDefault, passwordDefault)
    .WithManagementPlugin();

var postgres = builder.AddPostgres("postgres", userNameDefault, passwordDefault)
    .WithDataVolume();

var grafana = builder.AddContainer("grafana", "grafana/grafana")
    .WithBindMount("../grafana/config", "/etc/grafana")
    .WithBindMount("../grafana/dashboards", "/var/lib/grafana/dashboards")
    .WithBindMount("../grafana/data", "/var/lib/grafana")
    .WithEndpoint(port: 3000, targetPort: 3000, name: "grafana-http", scheme: "http");

builder.AddContainer("prometheus", "prom/prometheus")
    .WithBindMount("../prometheus", "/etc/prometheus")
    .WithBindMount("../prometheus/data", "/prometheus")
    .WithEndpoint(port: 9090, targetPort: 9090);

var elastic = builder.AddElasticsearch("elasticsearch", passwordDefault)
    .WithEnvironment("xpack.security.enabled", "false")
    .WithDataVolume();

var kibana = builder.AddContainer("kibana", "kibana","8.15.3")
    .WithBindMount("../kibana/config", "/usr/share/kibana/config")
    .WithBindMount("../kibana/data", "/usr/share/kibana/data")
    .WithReference(elastic)
    .WithEnvironment("ELASTICSEARCH_HOSTS", elastic.GetEndpoint("http"))
    .WithEndpoint(port: 5601, targetPort: 5601, name: "kibana-http", scheme: "http");

var notificationPostgres = postgres.AddDatabase("postgresNotifications", "notifications");
var mailPostgres = postgres.AddDatabase("postgresMails", "mails");
var smsPostgres = postgres.AddDatabase("postgresSms", "sms");
var pushPostgres = postgres.AddDatabase("postgresPush", "push");

var apiNotifications = builder.AddProject<Projects.Notification_Api>("apiNotifications")
    .WithReference(notificationPostgres)
    .WithReference(rabbitMq)
    .WithReference(elastic);

var apiMail = builder.AddProject<Projects.Mail_Api>("apiMail")
    .WithReference(mailPostgres)
    .WithReference(rabbitMq)
    .WithEnvironment("GRAFANA_URL", grafana.GetEndpoint("grafana-http"))
    .WithReference(apiNotifications);

var apiSms = builder.AddProject<Projects.Sms_Api>("apiSms")
    .WithReference(smsPostgres)
    .WithReference(rabbitMq)
    .WithReference(apiNotifications);

var apiPush = builder.AddProject<Projects.Push_Api>("apiPush")
    .WithReference(pushPostgres)
    .WithReference(rabbitMq)
    .WithReference(apiNotifications);

builder.Build().Run();