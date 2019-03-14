export interface Ticket {
    id: number;
	description:string;
    subject:string;
    applicantId:number;
    priority:number;
    status: TicketStatus;
    createdOn: Date;
    lastModified: Date;
    reactions: TicketReaction[];
    applicant: DiscordUser;
    assignees: TicketUser[];
}

export enum TicketStatus {
    open = 0,
    closed = 1,
    inProgress = 2
}

export enum TicketCategory {
    discordBots = 0,
    unity = 1,
    python = 2,
    web = 3,
    cSharp = 4,
    undecided = 5
}

export interface TicketReaction {
    id: number;
    ticketId: number;
    fromId: number;
    discordMessageId: number;
    ticket: Ticket;
    from: DiscordUser;
    discordMessage: DiscordMessage;
}

export interface TicketUser {

    ticketId: number;
    ticket: Ticket;

    discordUserId: number;
    discordUser: DiscordUser;
}

export interface DiscordUser {
    id: number;
    discordId: string;
    username: string;
    name: string;
    xp: number;
    level: number;
    ticketUsers: TicketUser[];
}

export interface DiscordMessage {
    id: number;
    messageId: string;
    guildId: string;
    channelId: string;
    message: string;
    timestamp: Date;
    isEmbed: boolean;
    isDm: boolean;
    imageLink: string;
    discordUserId: number;
    discordUser: DiscordUser;
}

export interface TicketAction {
    url: string;
    action: string;
}


