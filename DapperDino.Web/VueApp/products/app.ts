import Vue from 'vue'
import VueRouter from 'vue-router'
import Products from './views/Products.vue'
import Product from './views/Product.vue'

Vue.config.performance = true

Vue.use(VueRouter)
Vue.component('Products', Products);
Vue.component('Product', Product);

import store from './store/store'
import { router } from './router/router'

import App from './App.vue'

new Vue({
	store,
	router,
	render: h => h(Products)
}).$mount('#app')
