import Vue from "vue";
import Vuex, { StoreOptions } from "vuex";
import { GetterTree } from "vuex";
import { ActionTree } from "vuex";
import { MutationTree } from "vuex";
import { RootState } from "./state";

import axios, { AxiosRequestConfig } from "axios";
import {
    Ticket,
    TicketReaction,
    TicketAction,
    DiscordUser
} from "../types/models/ticket";
import { promises } from "fs";

const rp = require("request-promise")

Vue.use(Vuex);

const state: RootState = {
    ticket: {} as Ticket,
    tickets: new Array() as Ticket[],
    ticketActions: new Array() as TicketAction[],
    user: {} as DiscordUser,
    reactionUserInformation: []
};

const getters: GetterTree<RootState, RootState> = {
    ticket(state: RootState): Ticket {
        return state.ticket;
    },

    tickets(state: RootState): Ticket[] {
        return state.tickets || [];
    },

    ticketActions(state: RootState): TicketAction[] {
        return state.ticketActions || [];
    },
    user(state: RootState): DiscordUser {
        return state.user;
    },
    reactionUserInformation(state: RootState): [] {
        return state.reactionUserInformation;
    }
};

const actions: ActionTree<RootState, RootState> = {
    getCurrentUser({ commit }): Promise<boolean> {
        return new Promise((resolve, reject) => {
            axios
                .get("/api/tickets/GetCurrentDiscordUser/")
                .then(response => {
                    const payload: DiscordUser = response.data;
                    commit("SET_USER", payload);
                    resolve(true);
                })
                .catch(error => {
                    reject(error);
                });
        });
    },
    getTicketById({ commit }, id: number): Promise<boolean> {
        return new Promise((resolve, reject) => {
            axios
                .get("/api/tickets/GetById/" + id)
                .then(response => {
                    const payload: Ticket = response.data.ticket;
                    commit("SET_TICKET", payload);
                    commit("SET_REACTION_INFORMATION", response.data.reactionInformation);
                    resolve(true);
                })
                .catch(error => {
                    reject(error);
                });
        });
    },
    getQuickToolsForId({ commit }, id: number): Promise<boolean> {
        return new Promise((resolve, reject) => {
            axios
                .get("/api/tickets/GetTicketActions/" + id)
                .then(response => {
                    const payload: TicketAction[] = response.data;
                    commit("SET_TICKET_ACTIONS", payload);
                    resolve(true);
                })
                .catch(error => {
                    reject(error);
                });
        });
    },
    doAction({ commit }, action: TicketAction): Promise<boolean> {
        let self = this;
        return new Promise((resolve, reject) => {
            axios
                .get(action.url + self.state.ticket.id)
                .then(response => {
                    const payload: Ticket[] = response.data;
                    commit("SET_TICKET", payload);
                    resolve(true);
                })
                .catch(error => {
                    reject(error);
                });
        });
    },

    getTickets({ commit }): Promise<boolean> {
        return new Promise((resolve, reject) => {
            axios
                .get("/api/tickets/GetOpenTickets")
                .then(response => {
                    const payload: Ticket[] = response.data;
                    commit("SET_TICKETS", payload);
                    resolve(true);
                })
                .catch(error => {
                    reject(error);
                });
        });
    },

    setFirstTicket({ commit }): Promise<boolean> {
        return new Promise((resolve, reject) => {
            if (this.state.tickets.length > 0) {
                commit("SET_TICKET", this.state.tickets[0]);
                resolve(true);
            } else {
                reject("No tickets found");
            }
        });
    },

    addTicket({ commit }, ticket: Ticket): Promise<boolean> {
        return new Promise((resolve, reject) => {
            try {
                commit("ADD_TICKET", ticket);
                resolve(true);
            } catch (err) {
                reject(err);
            }
        });
    },

    sendMessage({ commit }, message: string): Promise<any> {
        const getUrl = window.location;
        const baseUrl = getUrl.protocol + "//" + getUrl.host;
        const options = {
            method: "POST",
            body: { message, ticketId: this.state.ticket.id },
            uri: baseUrl + "/Api/Tickets/SendMessage",
            json: true
        };

        return rp(options);
    },

    addReaction({ commit }, ticketReaction: TicketReaction): Promise<boolean> {
        return new Promise((resolve, reject) => {
            try {
                commit("ADD_REACTION", ticketReaction);
                resolve(true);
            } catch (err) {
                reject(err);
            }
        });
    },

    startup({ commit }, id: number | null): Promise<boolean> {
        return new Promise(async (resolve, reject) => {
            await axios
                .get("/api/tickets/GetOpenTickets")
                .then(async response => {
                    const openTickets = response.data as Ticket[];

                    let getId = () => {
                        if (id) return id;
                        if (openTickets && openTickets.length > 0) return openTickets[0].id;
                        return 1200;
                    };

                    try {
                        let id = getId();

                        await axios.get("/api/tickets/getbyid/" + id).then(async resp => {
                            const ticket: Ticket = resp.data.ticket;
                            const reactions = resp.data.reactionInformation;

                            await axios
                                .get("/api/tickets/GetTicketActions/" + id)
                                .then(response => {
                                    const payload: TicketAction[] = response.data;
                                    commit("SET_TICKET_ACTIONS", payload);
                                    commit("SET_TICKETS", openTickets);
                                    commit("SET_TICKET", ticket);
                                    commit("SET_REACTION_INFORMATION", reactions);
                                    resolve(true);
                                })
                                .catch(error => {
                                    reject(error);
                                });
                        });
                    } catch (e) {
                        reject(e);
                    }
                })
                .catch(error => {
                    reject(error);
                });
        });
    }
};

const mutations: MutationTree<RootState> = {
    SET_TICKET(state: RootState, ticket: Ticket) {
        state.ticket = ticket;
    },
    SET_TICKETS(state: RootState, tickets: Ticket[]) {
        state.tickets = tickets;
    },
    SET_REACTION_INFORMATION(state: RootState, reactionInformation: []) {
        state.reactionUserInformation = reactionInformation;
    },
    SET_TICKET_ACTIONS(state: RootState, ticketActions: TicketAction[]) {
        state.ticketActions = ticketActions;
    },
    ADD_TICKET(state: RootState, ticket: Ticket) {
        state.tickets.push(ticket);
    },
    ADD_REACTION(state: RootState, ticketReaction: TicketReaction) {
        if (state.ticket.id == ticketReaction.ticketId) {
            if (!state.ticket.reactions) {
                state.ticket.reactions = [];
            }

            state.ticket.reactions.push(ticketReaction);
        }
    },
    SET_USER(state: RootState, user: DiscordUser) {
        state.user = user;
    }
};

const store: StoreOptions<RootState> = {
    state,
    getters,
    actions,
    mutations
};

export default new Vuex.Store<RootState>(store);
