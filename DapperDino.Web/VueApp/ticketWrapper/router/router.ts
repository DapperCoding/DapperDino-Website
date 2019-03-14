import VueRouter from 'vue-router'

// Pages
import TicketWrapper from '@/ticketWrapper/views/TicketWrapper.vue'
import AddTicket from '@/ticketWrapper/views/AddTicket.vue'

const routePrefix = 'ticket'

const routes = [
	{
		path: '*',
        component: TicketWrapper
	},
	{
        name: 'ticketWrapper',
        path: `ticket`,
        component: TicketWrapper
    },
    {
        name: 'add',
        path: `add`,
        component: AddTicket
    }
]

export const router = new VueRouter({
	mode: 'history',
	routes,
	linkActiveClass: 'is-active'
})
