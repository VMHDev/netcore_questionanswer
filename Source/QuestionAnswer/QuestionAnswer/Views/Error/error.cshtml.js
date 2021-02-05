import Vue from 'vue';
import "@Views/app.js";
import PageNotFound from '@Components/General/PageNotFound.vue';
Vue.component('PageNotFound', PageNotFound)
import PageForbidden from '@Components/General/PageForbidden.vue';
Vue.component('PageForbidden', PageForbidden)

new Vue({
    el: "#app",
    created: function () {
        this.showmenuItem = "NotFound";
        let lstParam = this.getParameterURL();
        if (lstParam.page && lstParam.page != "") {
            this.showmenuItem = lstParam.page;
        }
    }
});