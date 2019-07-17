<template>

    <div class="container">
        <div class="row">
            <div class="col-xs-12">
                <h2 style="text-align:center">Create a ticket</h2>
            </div>
        </div>

        <div class="row">
            <div class="col-xs-12">
                <form id="sendMessageForm" @submit="createTicket">
                    <div class="form-group">
                        <label for="subject">Please fill in a subject for your ticket</label>
                        <input class="form-control" type="text" name="subject" id="subject" v-model="subject" placeholder="Your subject">
                    </div><!-- /input-group -->

                    <div class="form-group">
                        <label for="description">Please fill in a description for your ticket</label>
                        <input class="form-control" type="text" name="description" id="description" v-model="description" placeholder="Your subject">
                    </div>

                    <input class="btn btn-primary" type="submit" value="Send">

                </form>
            </div>
        </div>


    </div>

</template>

<script lang="ts">

    import { Vue, Component } from 'vue-property-decorator'
    import { mapGetters } from 'vuex'

    import { Notification } from "@/common/services/notificationService";
    import { Ticket, TicketReaction, TicketAction } from '../types/models/ticket';
    import * as aspnet from "@aspnet/signalr";
    import { log } from 'util';
    import axios, { AxiosRequestConfig } from 'axios'

    @Component({
        name: 'AddTicket',
        computed: {
            ...mapGetters(['user'])
        },

        data: {
            subject: "",
            description: ""
        },
        methods: {
            created() {


            },
            createTicket(e) {
                e.preventDefault();
                console.log("create ticket submit");

                // TODO: Create ticket through API
                let self = this as AddTicket;
                console.log(self);

                const data = { title: self.subject, description: self.description };
                const options = {
                    method: 'POST',
                    headers: { 'content-type': 'application/x-www-form-urlencoded' },
                    data:data,
                    url: "/Api/Tickets/Create"
                };
                axios(options)
                    .then((response: { data: Ticket }) => {
                        let href = window.location.href.toLowerCase();
                        if (href.includes("happytohelp")) {
                            window.location.href = "/HappyToHelp/Ticket?id=" + response.data.id;
                        } else if (href.includes("admin")) {
                            window.location.href = "/Admin/Ticket?id=" + response.data.id;
                        } else {
                            window.location.href = "/Client/Ticket?id=" + response.data.id;
                        }

                        
                    })
                    .catch(console.error);

              
            }
        }
    })
    export default class AddTicket extends Vue {

        subject: string = "";
        description: string = "";
    }
</script>

<style scoped>
</style>
