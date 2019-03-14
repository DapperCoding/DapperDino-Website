import VueRouter from 'vue-router'

// Pages
import Products from '@/products/views/Products.vue'

const routePrefix = 'products'

const routes = [
	{
		path: '*',
        component: Products
	},
	{
        name: 'ticketWrapper',
		path: `${routePrefix}`,
        component: Products
    }
]

export const router = new VueRouter({
	mode: 'history',
	routes,
	linkActiveClass: 'is-active'
})
