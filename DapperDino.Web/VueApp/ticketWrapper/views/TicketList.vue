<template>

    <ul>
        <li v-for="(item, index) in tickets" :class="item.id == currentId ? 'selected':''">
            <p @click="loadTicket(item.id)">
                ticket{{ item.id }} ({{ reactionUserInformation.find(x=>x.discordId == item.applicant.discordId) != null ? reactionUserInformation.find(x=>x.discordId == item.applicant.discordId).status : 'offline' }})
            </p>
        </li>
    </ul>


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
        name: 'TicketList',
        computed: {
            ...mapGetters(['tickets']),
            ...mapGetters(['reactionUserInformation'])
        }
    })
    export default class TicketList extends Vue {

        currentId = 0;

        created() {
            this.currentId = this.$props.ticketId;
        }

        doAction(action: TicketAction): void {
            axios
                .get(action.url)
                .then(response => {
                    console.log(action.action)
                    console.log(response);

                    if (response.data && response.data.refresh) {
                        this.fetchTicket();
                        this.fetchQuickTools();
                    }
                })
                .catch(error => {
                    console.error(error);
                })
        }

        fetchTicket(): void {
            this.$store.dispatch('getTicketById', this.currentId)
                .then(result => {

                    Notification.success(this, 'Data fetched successfully!')

                }).catch(error => {

                    Notification.error(this, 'Error fetching data!')

                })

        }

        fetchQuickTools(): void {
            this.$store.dispatch('getQuickToolsForId', this.currentId)
                .then(result => {

                    Notification.success(this, 'Data fetched successfully!')

                }).catch(error => {

                    Notification.error(this, 'Error fetching data!')

                })

        }

        loadTicket(id: number): void {
            this.currentId = id;

            this.fetchTicket();
            this.fetchQuickTools();
        }
    }
</script>

<style>
</style>
