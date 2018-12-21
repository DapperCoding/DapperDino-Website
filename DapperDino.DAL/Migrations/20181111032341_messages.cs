using Microsoft.EntityFrameworkCore.Migrations;

namespace DapperDino.DAL.Migrations
{
    public partial class messages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_TicketReactions_DiscordUsers_FromId",
            //    table: "TicketReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketReactions_Tickets_TicketId",
                table: "TicketReactions");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Tickets_DiscordUsers_AssignedToId",
            //    table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "TicketReactions");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "TicketReactions");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "FrequentlyAskedQuestions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DiscordMessages");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "Tickets",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
            

            migrationBuilder.AddColumn<int>(
                name: "DiscordMessageId",
                table: "TicketReactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DiscordMessageId",
                table: "FrequentlyAskedQuestions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscordUserId",
                table: "DiscordMessages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TicketReactions_DiscordMessageId",
                table: "TicketReactions",
                column: "DiscordMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_FrequentlyAskedQuestions_DiscordMessageId",
                table: "FrequentlyAskedQuestions",
                column: "DiscordMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordMessages_DiscordUserId",
                table: "DiscordMessages",
                column: "DiscordUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordMessages_DiscordUsers_DiscordUserId",
                table: "DiscordMessages",
                column: "DiscordUserId",
                principalTable: "DiscordUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FrequentlyAskedQuestions_DiscordMessages_DiscordMessageId",
                table: "FrequentlyAskedQuestions",
                column: "DiscordMessageId",
                principalTable: "DiscordMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_TicketReactions_DiscordMessages_DiscordMessageId",
            //    table: "TicketReactions",
            //    column: "DiscordMessageId",
            //    principalTable: "DiscordMessages",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketReactions_DiscordUsers_FromId",
                table: "TicketReactions",
                column: "FromId",
                principalTable: "DiscordUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketReactions_Tickets_TicketId",
                table: "TicketReactions",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_DiscordUsers_AssignedToId",
                table: "Tickets",
                column: "AssignedToId",
                principalTable: "DiscordUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordMessages_DiscordUsers_DiscordUserId",
                table: "DiscordMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_FrequentlyAskedQuestions_DiscordMessages_DiscordMessageId",
                table: "FrequentlyAskedQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketReactions_DiscordMessages_DiscordMessageId",
                table: "TicketReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketReactions_DiscordUsers_FromId",
                table: "TicketReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketReactions_Tickets_TicketId",
                table: "TicketReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_DiscordUsers_AssignedToId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_TicketReactions_DiscordMessageId",
                table: "TicketReactions");

            migrationBuilder.DropIndex(
                name: "IX_FrequentlyAskedQuestions_DiscordMessageId",
                table: "FrequentlyAskedQuestions");

            migrationBuilder.DropIndex(
                name: "IX_DiscordMessages_DiscordUserId",
                table: "DiscordMessages");

            migrationBuilder.DropColumn(
                name: "DiscordMessageId",
                table: "TicketReactions");

            migrationBuilder.DropColumn(
                name: "DiscordMessageId",
                table: "FrequentlyAskedQuestions");

            migrationBuilder.DropColumn(
                name: "DiscordUserId",
                table: "DiscordMessages");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "Tickets",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tickets",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "TicketReactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MessageId",
                table: "TicketReactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MessageId",
                table: "FrequentlyAskedQuestions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "DiscordMessages",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketReactions_DiscordUsers_FromId",
                table: "TicketReactions",
                column: "FromId",
                principalTable: "DiscordUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketReactions_Tickets_TicketId",
                table: "TicketReactions",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_DiscordUsers_AssignedToId",
                table: "Tickets",
                column: "AssignedToId",
                principalTable: "DiscordUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
