<template>

        <ul>
            <li v-for="(item, index) in ticketActions">
                <p @click="doAction(item)">{{ item.action }}</p>
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
        name: 'QuickTools',
        computed: {
            ...mapGetters(['ticketActions'])
        },
        props: ['ticketId']
    })
    export default class QuickTools extends Vue {
        currentId = 0;

        created() {
            this.currentId = this.$props.ticketId;
            this.fetchQuickTools();
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

                    Notification.success(this, 'Messages successfully loaded!')

                }).catch(error => {

                    Notification.error(this, 'Error fetching messages!')

                })

        }

        fetchQuickTools(): void {
            this.$store.dispatch('getQuickToolsForId', this.currentId)
                .then(result => {

                }).catch(error => {

                    Notification.error(this, 'Error fetching quick tools!')

                })

        }

        loadTicket(id: number): void {
            this.currentId = id;

            this.fetchTicket();
            this.fetchQuickTools();
        }
    }
</script>

<style scoped>
</style>
