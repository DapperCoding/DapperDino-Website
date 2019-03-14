<template>

    <div v-if="ticket.reactions">
        <div class="row" v-for="(item, index) in ticket.reactions.slice().reverse()" style="margin-top:15px;">
            <div v-if="item.from.discordId === user.discordId" class="col-xs-6"></div>
            <div class="col-xs-6">

                <div class="media clearfix">
                    <div v-bind:class="item.from.discordId !== user.discordId ? 'media-left' : 'media-right'" v-bind:style="{'float': item.from.discordId !== user.discordId ? 'left' : 'right'}">
                        <img class="media-object" src="/images/favicon.png" alt="..." style="max-width:50px;">
                    </div>
                    <div class="media-body" v-bind:style="{'float': item.from.discordId !== user.discordId ? 'left' : 'right'}">
                        <h4 class="media-heading">{{item.from != null ? item.from.username : ""}}</h4>
                        <p>
                            {{item.discordMessage != null ? item.discordMessage.message: ""}}
                        </p>
                    </div>
                </div>

            </div>
            <div v-if="item.from.discordId !== user.discordId" class="col-xs-6"></div>
        </div>
    </div>

    <!-- https://codepen.io/drehimself/pen/KdXwxR -->

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
        name: 'TicketReactions',
        computed: {
            ...mapGetters(['ticket']),
            ...mapGetters(['user'])
        }
    })
    export default class TicketReactions extends Vue {
        
        created() {

            
        }
    }
</script>

<style>
    .media-right {
        text-align:right;
    }
</style>
