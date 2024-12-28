"use strict";(self.webpackChunkgrafana_lokiexplore_app=self.webpackChunkgrafana_lokiexplore_app||[]).push([[599],{4599:(e,t,r)=>{r.r(t),r.d(t,{default:()=>w,updatePlugin:()=>h});var n=r(8531),a=r(7781),o=r(1269),i=r(6089),l=r(2007),c=r(5959),s=r.n(c),u=r(3241),p=r(2871);function d(e,t,r,n,a,o,i){try{var l=e[o](i),c=l.value}catch(e){return void r(e)}l.done?t(c):Promise.resolve(c).then(n,a)}function f(e){return function(){var t=this,r=arguments;return new Promise((function(n,a){var o=e.apply(t,r);function i(e){d(o,n,a,i,l,"next",e)}function l(e){d(o,n,a,i,l,"throw",e)}i(void 0)}))}}function g(e,t,r){return t in e?Object.defineProperty(e,t,{value:r,enumerable:!0,configurable:!0,writable:!0}):e[t]=r,e}const m=e=>({colorWeak:i.css`
    color: ${e.colors.text.secondary};
  `,marginTop:i.css`
    margin-top: ${e.spacing(3)};
  `,marginTopXl:i.css`
    margin-top: ${e.spacing(6)};
  `,label:(0,i.css)({display:"flex",alignItems:"center",marginBottom:e.spacing(.75)}),icon:(0,i.css)({marginLeft:e.spacing(1)})}),v=(b=f((function*(e,t){try{yield h(e,t),n.locationService.reload()}catch(e){p.v.error("Error while updating the plugin")}})),function(e,t){return b.apply(this,arguments)});var b;const y={container:"data-testid ac-container",interval:"data-testid ac-interval-input",submit:"data-testid ac-submit-form"},h=function(){var e=f((function*(e,t){const r=(0,n.getBackendSrv)().fetch({url:`/api/plugins/${e}/settings`,method:"POST",data:t});return(yield(0,o.lastValueFrom)(r)).data}));return function(t,r){return e.apply(this,arguments)}}(),O=e=>{try{if(e){const t=a.rangeUtil.intervalToSeconds(e);return(0,u.isNumber)(t)&&t>=3600}return!0}catch(e){}return!1},w=({plugin:e})=>{const t=(0,l.useStyles2)(m),{enabled:r,pinned:n,jsonData:a}=e.meta;var o,i;const[u,p]=(0,c.useState)({interval:null!==(o=null==a?void 0:a.interval)&&void 0!==o?o:"",isValid:O(null!==(i=null==a?void 0:a.interval)&&void 0!==i?i:"")});return s().createElement("div",{"data-testid":y.container},s().createElement(l.FieldSet,{label:"Settings"},s().createElement(l.Field,{invalid:!O(u.interval),error:'Interval is invalid. Please enter an interval longer then "60m". For example: 3d, 1w, 1m',description:s().createElement("span",null,"The maximum interval that can be selected in the time picker within the Explore Logs app. If empty, users can select any time range interval in Explore Logs. ",s().createElement("br",null),"Example values: 7d, 24h, 2w"),label:"Maximum time picker interval",className:t.marginTop},s().createElement(l.Input,{width:60,id:"interval","data-testid":y.interval,label:"Max interval",value:null==u?void 0:u.interval,placeholder:"7d",onChange:e=>{const t=e.target.value.trim();var r,n;p((r=function(e){for(var t=1;t<arguments.length;t++){var r=null!=arguments[t]?arguments[t]:{},n=Object.keys(r);"function"==typeof Object.getOwnPropertySymbols&&(n=n.concat(Object.getOwnPropertySymbols(r).filter((function(e){return Object.getOwnPropertyDescriptor(r,e).enumerable})))),n.forEach((function(t){g(e,t,r[t])}))}return e}({},u),n=null!=(n={interval:t,isValid:O(t)})?n:{},Object.getOwnPropertyDescriptors?Object.defineProperties(r,Object.getOwnPropertyDescriptors(n)):function(e,t){var r=Object.keys(e);if(Object.getOwnPropertySymbols){var n=Object.getOwnPropertySymbols(e);r.push.apply(r,n)}return r}(Object(n)).forEach((function(e){Object.defineProperty(r,e,Object.getOwnPropertyDescriptor(n,e))})),r))}})),s().createElement("div",{className:t.marginTop},s().createElement(l.Button,{type:"submit","data-testid":y.submit,onClick:()=>v(e.meta.id,{enabled:r,pinned:n,jsonData:{interval:u.interval}}),disabled:!O(u.interval)},"Save settings"))))}},2871:(e,t,r)=>{r.d(t,{v:()=>l});var n=r(8531);function a(e,t,r){return t in e?Object.defineProperty(e,t,{value:r,enumerable:!0,configurable:!0,writable:!0}):e[t]=r,e}function o(e){for(var t=1;t<arguments.length;t++){var r=null!=arguments[t]?arguments[t]:{},n=Object.keys(r);"function"==typeof Object.getOwnPropertySymbols&&(n=n.concat(Object.getOwnPropertySymbols(r).filter((function(e){return Object.getOwnPropertyDescriptor(r,e).enumerable})))),n.forEach((function(t){a(e,t,r[t])}))}return e}const i={app:r(2533).id,version:"1.0.4"},l={info:(e,t)=>{const r=o({},i,t);console.log(e,r),c(e,r)},warn:(e,t)=>{const r=o({},i,t);console.warn(e,r),s(e,r)},error:(e,t)=>{const r=o({},i,t);console.error(e,r),u(e,r)}},c=(e,t)=>{try{(0,n.logInfo)(e,t)}catch(e){console.warn("Failed to log faro event!")}},s=(e,t)=>{try{(0,n.logWarning)(e,t)}catch(r){console.warn("Failed to log faro warning!",{msg:e,context:t})}},u=(e,t)=>{let r=t;try{!function(e,t){if("object"==typeof e&&null!==e&&("object"==typeof e&&Object.keys(e).forEach((r=>{const n=e[r];"string"!=typeof n&&"boolean"!=typeof n&&"number"!=typeof n||(t[r]=n.toString())})),p(e)))if("object"==typeof e.data&&null!==e.data)try{t.data=JSON.stringify(e.data)}catch(e){}else"string"!=typeof e.data&&"boolean"!=typeof e.data&&"number"!=typeof e.data||(t.data=e.data.toString())}(e,r),e instanceof Error?(0,n.logError)(e,r):"string"==typeof e?(0,n.logError)(new Error(e),r):e&&"object"==typeof e?r.msg?(0,n.logError)(new Error(r.msg),r):(0,n.logError)(new Error("error object"),r):(0,n.logError)(new Error("unknown error"),r)}catch(t){console.error("Failed to log faro error!",{err:e,context:r})}},p=e=>"data"in e}}]);
//# sourceMappingURL=599.js.map