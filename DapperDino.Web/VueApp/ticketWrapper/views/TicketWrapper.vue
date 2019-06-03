<template>


    <div class="">
        <!--<div class="columns">
            <div class="column">
                <h6>{{ tickets.length }} open tickets</h6>
                <ul>
                    <li>open tickets</li>
                    <li>closed tickets</li>
                </ul>
            </div>
            <div class="column">
                <div class="columns">
                    <div class="column">
                        <h2>{{ ticket.subject }}</h2>
                        <p>{{ ticket.description }}</p>
                    </div>
                </div>


            </div>
            <div class="column">
                <h2>Quick Tools</h2>
            </div>

        </div>-->
            <div class="left-menu">

                <TicketList v-bind:ticketId="ticketId"></TicketList>

            </div>
            <div class="reaction-wrapper">

                <TicketReactions></TicketReactions>

            </div>
            <div class="right-menu">
                <QuickTools v-bind:ticketId="ticketId"></QuickTools>
            </div>

        <div class="columns sticky-footer no-margins">
            <div class="column is-one-fifth"></div>
            <div class="column is-three-fifths">
                <form id="sendMessageForm" @submit="sendMessage">
                    <div class="input-group">
                        <input class="form-control" type="text" name="description" id="description" v-model="chatmessage" placeholder="Type your message here">
                        <span class="input-group-btn">
                            <input class="btn btn-primary" type="submit" value="Send">
                        </span>
                    </div><!-- /input-group -->

                </form>
            </div>
            <div class="column is-one-fifth"></div>
        </div>
        <notifications group="TicketWrapper" />
    </div>
</template>

<script lang="ts">

    import { Vue, Component } from 'vue-property-decorator'
    import { mapGetters } from 'vuex'

    import { Notification } from "@/common/services/notificationService";
    import { Ticket, TicketReaction, TicketAction } from '../types/models/ticket';
    import * as aspnet from "@aspnet/signalr";
    import { log } from 'util';
    import axios from 'axios'
import { connect } from 'http2';

    @Component({
        name: 'TicketWrapper',
        computed: {
            ...mapGetters(['tickets']),
            ...mapGetters(['ticket'])
        }
    })
    export default class TicketWrapper extends Vue {
        chatmessage: string = "";
        ticketId: number = 1200;

        created() {
            let id = this.getParameterByName("id") || null;

            if (id) {
                try {
                    this.ticketId = parseInt(id);
                } catch (e) {

                }
            }

            //this.fetchTickets();
            //this.fetchTicket();
            this.startup();

            this.startupWebConnection().then(a => console.log("started web service")).catch(console.error);
            this.startupApiConnection().then(a => console.log("started api server")).catch(console.error);
        }

        reverse(arr: []) {
            return arr ? arr.reverse() : [];
        }

        getParameterByName(name: string, url?: string) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, '\\$&');
            var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, ' '));
        }

        startup(): void {
            let id = this.getParameterByName("id") || null;

            this.$store.dispatch('getCurrentUser', id)
                .then(result => {

                    //Notification.success(this, 'Setup completed')

                }).catch(error => {

                    Notification.error(this, 'Wasn\'t able to fetch the current user!')

                })

            this.$store.dispatch('startup', id)
                .then(result => {

                    Notification.success(this, 'Setup completed')

                }).catch(error => {

                    Notification.error(this, 'Error fetching data!')

                })
        }

        sendMessage(e: any): void {
            e.preventDefault();
            this.$store.dispatch("sendMessage", this.chatmessage)
                .then(result => {
                    Notification.success(this, 'Message sent')
                    this.chatmessage = "";
                }).catch(error => {
                    Notification.success(this, 'Whoops can\'t send message.')
                })
        }

        addTicket(ticket: Ticket): void {

            this.$store.dispatch('addTicket', ticket)
                .then(result => {

                    Notification.success(this, 'Data fetched successfully!')

                }).catch(error => {

                    Notification.error(this, 'Error fetching data!')

                })

        }

        addReaction(reaction: TicketReaction): void {
            this.$store.dispatch('addReaction', reaction)
                .then(result => {

                    Notification.success(this, `Added a reaction to ticket ${reaction.ticketId}!`)

                }).catch(error => {

                    Notification.error(this, 'Error adding ticketReaction!')

                })

        }

        async startupWebConnection(): Promise<boolean> {
            let self = this;
            return new Promise<boolean>(async (resolve, reject) => {
                // Creates connection to our Websites's SignalR hub
                const connection = new aspnet.HubConnectionBuilder()
                    .withUrl('/discordbothub')
                    .configureLogging(aspnet.LogLevel.Debug)
                    .build();

                // Start connection
                await connection.start()
                    .then(console.log)
                    .catch(console.error);

                connection.on("AddTicketReaction", () => { });

                // On 'TicketCreated' -> fires when ticket is created through Website
                connection.on("TicketCreated", async (ticket: Ticket) => {
                    self.addTicket(ticket);
                });

                // On 'TicketReaction' -> fires when ticket reaction has been added to an existing ticket
                connection.on("TicketReaction", async (reaction: TicketReaction) => {
                    self.addReaction(reaction);
                    setTimeout(() => {
                        let elements = document.getElementsByClassName("reaction-inner-wrapper");

                        if (elements && elements.length > 0) {
                            elements[0].parentElement.scrollTop = elements[0].parentElement.scrollHeight;
                        }
                    }, 1000)

                });

                connection.on("AddTicket", async (ticket: Ticket) => {
                    self.addTicket(ticket);
                });

                resolve(true);
            })
        }

        async startupApiConnection(): Promise<boolean> {
            let self = this;
            return new Promise<boolean>(async (resolve, reject) => {
                // Creates connection to our API's SignalR hub
                const connection = new aspnet.HubConnectionBuilder()
                    .withUrl('https://api.dapperdino.co.uk/discordbothub')
                    .configureLogging(aspnet.LogLevel.Debug)
                    .build();

                // Start connection
                await connection.start()
                    .then(console.log)
                    .catch(console.error);

                // On 'TicketCreated' -> fires when ticket is created through API
                connection.on("TicketCreated", async (ticket: Ticket) => {
                    console.log("Received ticket")
                    self.addTicket(ticket);
                });

                // On 'TicketReaction' -> fires when ticket reaction has been added to an existing ticket
                connection.on("TicketReaction", async (reaction: TicketReaction) => {
                    self.addReaction(reaction);
                    setTimeout(() => {
                        let elements = document.getElementsByClassName("reaction-inner-wrapper");

                        if (elements && elements.length > 0) {
                            elements[0].parentElement.scrollTop = elements[0].parentElement.scrollHeight;
                        }
                    }, 1000)
                });

                resolve(true);
            })
        }
    }
</script>

<style scoped>

    table.discordmessages {
        display: block;
        table-layout: fixed;
        word-wrap: break-word;
    }

        table.discordmessages tr,
        table.discordmessages td,
        table.discordmessages th,
        table.discordmessages tbody,
        table.discordmessages thead {
            display: block;
        }
</style>
