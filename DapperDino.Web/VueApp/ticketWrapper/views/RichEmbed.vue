<template>


    <div></div>
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
        name: 'RichEmbed',
        computed: {
        },
        props: ['embed']
    })
    export default class RichEmbed extends Vue {
       
    }
</script>

<style scoped>
</style>
