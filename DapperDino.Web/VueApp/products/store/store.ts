import Vue from 'vue'
import Vuex, { StoreOptions } from 'vuex'
import { GetterTree } from 'vuex'
import { ActionTree } from 'vuex'
import { MutationTree } from 'vuex'
import { RootState } from './state'

import axios from 'axios'
import { promises } from 'fs';

Vue.use(Vuex)

const state: RootState = {
}

const getters: GetterTree<RootState, RootState> = {
    
}

const actions: ActionTree<RootState, RootState> = {
    
}

const mutations: MutationTree<RootState> = {
   
}

const store: StoreOptions<RootState> = {
    state,
    getters,
    actions,
    mutations
}

export default new Vuex.Store<RootState>(store)
