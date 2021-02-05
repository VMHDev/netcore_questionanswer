import Vue from 'vue';
import "@Views/app.js";
import Home from '@Components/Home/Home.vue';
Vue.component('Home', Home)

new Vue({
    el: "#app",
    created: function () {

    }
});