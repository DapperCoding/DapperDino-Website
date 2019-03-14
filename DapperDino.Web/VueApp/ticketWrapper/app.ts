import Vue from 'vue'
import VueRouter from 'vue-router'

import QuickTools from '@/ticketWrapper/views/QuickTools.vue';
import TicketReactions from '@/ticketWrapper/views/TicketReactions.vue';
import TicketList from '@/ticketWrapper/views/TicketList.vue';

Vue.config.performance = true

Vue.use(VueRouter)

Vue.component('QuickTools', QuickTools);
Vue.component('TicketReactions', TicketReactions);
Vue.component('TicketList', TicketList);

import store from './store/store'
import { router } from './router/router'

import App from './App.vue'

new Vue({
	store,
	router,
	render: h => h(App)
}).$mount('#app')
