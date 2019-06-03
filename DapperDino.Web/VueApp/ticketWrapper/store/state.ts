import { Ticket, TicketAction, DiscordUser } from "../types/models/ticket";


export interface RootState {
    ticket: Ticket
    tickets: Ticket[]
    ticketActions: TicketAction[];
    user: DiscordUser;
    reactionUserInformation: []
}
