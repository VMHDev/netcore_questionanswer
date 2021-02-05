// <<<<<<<<<<<<<<<<<<<<<<< Khai báo các thư viện dùng chung cho toàn project >>>>>>>>>>>>>>>>>>>>>>>>>>>

// Using BootstrapVue
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import Vue from 'vue';
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'

Vue.use(BootstrapVue)
Vue.use(IconsPlugin)

// Import user components
import MainHeader from '@Components/Layouts/MainHeader.vue';
Vue.component('main-header', MainHeader);