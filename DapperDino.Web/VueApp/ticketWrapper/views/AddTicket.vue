<template>

    <div class="container">
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

</template>

<script lang="ts">

    import { Vue, Component } from 'vue-property-decorator'
    import { mapGetters } from 'vuex'

    import { Notification } from "@/common/services/notificationService";
    import { Ticket, TicketReaction, TicketAction } from '../types/models/ticket';
    import * as aspnet from "@aspnet/signalr";
    import { log } from 'util';
    import axios from 'axios'

    @Component({
        name: 'AddTicket',
        computed: {
            ...mapGetters(['user'])
        }
    })
    export default class AddTicket extends Vue {

        subject = "";
        description = "";

        created() {

            
        }

        createTicket() {
            console.log("create ticket submit");
        }
    }
</script>

<style scoped>

</style>
