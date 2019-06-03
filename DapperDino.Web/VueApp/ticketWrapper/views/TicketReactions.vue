<template>

    <div class="reaction-inner-wrapper" v-if="ticket.reactions && ticket.reactions.length > 0">
        
        <div class="reaction-row" v-for="(item, index) in ticket.reactions" style="margin-top:15px;">
            <div class="column reaction-inner-row">

                <div class="media clearfix reaction-author">
                    <div class="media-left" style="float:left;" >
                        <img class="media-object avatar" v-bind:src="reactionUserInformation.find(x=>x.discordId == item.from.discordId).avatarUrl" alt="..." >
                    </div>
                    <div class="media-body" style="float:left;">
                        <h4 class="media-heading">{{ reactionUserInformation.find(x=>x.discordId == item.from.discordId).username }} <small class="datetime">{{ item.discordMessage.timestamp }}</small></h4>
                        <p v-if="!item.discordMessage.embeds || item.discordMessage.embeds.length <= 0">
                            {{ item.discordMessage.message }}
                        </p>
                        <div class="clearfix" v-if="item.discordMessage.embeds && item.discordMessage.embeds.length > 0">
                            <div class="discord-embed clearfix" v-for="embed in item.discordMessage.embeds">
                                <div class="discord-header">
                                    <h2 class="discord-title">{{embed.title}}</h2>
                                    <p class="discord-description">{{embed.description}}</p>
                                </div>

                                <div v-if="embed.thumbnail" class="discord-thumbnail">
                                    <img :src="embed.thumbnail.url" :width="embed.thumbnail.width" :height="embed.thumbnail.height" alt="Image here"/>
                                </div>

                                <div class="discord-fields" v-if="embed.fields && embed.fields.length > 0">
                                    <div class="discord-field" v-for="(field, fieldIndex) in embed.fields" :class="{inlineField: field.inine}">
                                        <h2 class="discord-title">{{field.title}}</h2>
                                        <p class="discord-value">{{field.value}}</p>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>


</template>

<script lang="ts">

    import { Vue, Component } from 'vue-property-decorator'
    import { mapGetters, mapActions } from 'vuex'

    import { Notification } from "@/common/services/notificationService";
    import { Ticket, TicketReaction, TicketAction } from '../types/models/ticket';
    import * as aspnet from "@aspnet/signalr";
    import { log } from 'util';
    import axios from 'axios'
import { setTimeout } from 'timers';

    @Component({
        name: 'TicketReactions',
        computed: {
            ...mapGetters(['ticket']),
            ...mapGetters(['user']),
            ...mapGetters(['reactionUserInformation'])
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
