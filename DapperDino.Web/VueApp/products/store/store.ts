import Vue from 'vue'
import Vuex, { StoreOptions } from 'vuex'
import { GetterTree } from 'vuex'
import { ActionTree } from 'vuex'
import { MutationTree } from 'vuex'
import { RootState } from './state'
import { Product } from '@/products/types/models/product';

import axios from 'axios'
import { promises } from 'fs';

Vue.use(Vuex)

const state: RootState = {
    Products: [] as Product[]
}

const getters: GetterTree<RootState, RootState> = {
    products(state: RootState): Product[] {
        return state.Products || [];
    }
}

const actions: ActionTree<RootState, RootState> = {
    getProducts({ commit }): Promise<boolean> {
        return new Promise((resolve, reject) => {
            axios
                .get('/api/Products/GetProducts')
                .then(response => {
                    console.log(response);
                    const payload: Product[] = response.data
                    commit('SET_PRODUCTS', payload)
                    resolve(true)
                })
                .catch(error => {
                    reject(error)
                })
        })
    }
}

const mutations: MutationTree<RootState> = {
    SET_PRODUCTS(state: RootState, products: Product[]) {
        state.Products = products;
    },
}

const store: StoreOptions<RootState> = {
    state,
    getters,
    actions,
    mutations
}

export default new Vuex.Store<RootState>(store)
