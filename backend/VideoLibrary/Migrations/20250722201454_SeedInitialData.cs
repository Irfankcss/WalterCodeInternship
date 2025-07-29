using System;
using Microsoft.EntityFrameworkCore.Migrations;
using VideoLibrary.Helpers;

namespace VideoLibrary.Migrations
{
    public partial class SeedInitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Users
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Username", "Password", "Email", "Name", "Dob", "Admin", "IsDeleted" },
                values: new object[,]
                {
                    { 1, "admin", PasswordHasher.Hash("lozinka"), "admin@videolib.com", "Admin User", new DateTime(1985, 1, 1), true, false },
                    { 2, "irfan", PasswordHasher.Hash("lozinka"), "irfan@gmail.com", "Irfan Kurić", new DateTime(2002, 8, 27), false, false },
                    { 3, "emil", PasswordHasher.Hash("lozinka"), "emil@gmail.com", "Emil Ahmetović", new DateTime(2003, 3, 10), false, false },
                    { 4, "pavle", PasswordHasher.Hash("lozinka"), "pavle@gmail.com", "Pavle Klisura", new DateTime(1992, 8, 15), false, false }
                    
                });

            // Directors
            migrationBuilder.InsertData(
                table: "Directors",
                columns: new[] { "Id", "Name", "Dob", "Bio", "IsDeleted" },
                values: new object[,]
                {
                    { 1, "Christopher Nolan", new DateTime(1970, 7, 30), "Sir Christopher Edward Nolan is a British and American filmmaker. Known for his Hollywood blockbusters with structurally complex storytelling, he is considered a leading filmmaker of the 21st century. Nolan's films have earned over $6.6 billion worldwide, making him the seventh-highest-grossing film director.", false },
                    { 2, "Steven Spielberg", new DateTime(1946, 12, 18), "Steven Allan Spielberg is an American filmmaker. A major figure of the New Hollywood era and pioneer of the modern blockbuster, Spielberg is widely regarded as one of the greatest and most influential filmmakers in the history of cinema and is the highest-grossing film director of all time.", false },
                    { 3, "Quentin Tarantino", new DateTime(1963, 3, 27), "Quentin Jerome Tarantino is an American filmmaker, actor, and author. His films are characterized by graphic violence, extended dialogue often featuring much profanity, and references to popular culture.", false }
                });

            // Actors
            migrationBuilder.InsertData(
                table: "Actors",
                columns: new[] { "Id", "Name", "Dob", "Bio", "IsDeleted" },
                values: new object[,]
                {
                    { 1, "Leonardo DiCaprio", new DateTime(1974, 11, 11), "Leonardo Wilhelm DiCaprio is an American actor and film producer. Known for his work in biographical and period films, he is the recipient of numerous accolades, including an Academy Award, a British Academy Film Award, and three Golden Globe Awards.", false },
                    { 2, "Scarlett Johansson", new DateTime(1984, 11, 22), "Scarlett Ingrid Johansson is an American actress and singer. Her films as a leading actress have grossed over $15.1 billion worldwide, making her the highest-grossing lead actor in history.", false },
                    { 3, "Tom Hanks", new DateTime(1956, 7, 9), "Thomas Jeffrey Hanks is an American actor and filmmaker. Known for both his comedic and dramatic roles, he is one of the most popular and recognizable film stars worldwide, and is regarded as an American cultural icon. Hanks is ranked as the fourth-highest-grossing American film actor.", false },
                    { 4, "Brad Pitt", new DateTime(1963, 12, 18), "William Bradley Pitt is an American actor and film producer. In a film career spanning more than thirty years, Pitt has received numerous accolades, including two Academy Awards, two British Academy Film Awards, two Golden Globe Awards, and a Primetime Emmy Award.", false },
                    { 5, "Natalie Portman", new DateTime(1981, 6, 9), "Natalie Hershlag, known professionally as Natalie Portman, is an actress, film producer and director with dual Israeli and American citizenship.", false },
                    { 6, "Morgan Freeman", new DateTime(1937, 6, 1), "Morgan Freeman is an American actor, producer, and narrator. In a career spanning six decades, he has received numerous accolades, including an Academy Award and a Golden Globe Award, as well as a nomination for a Tony Award.", false }
                });

            // Genres
            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name", "Description", "IsDeleted" },
                values: new object[,]
                {
                    { 1, "Action", "Fast-paced movies with physical stunts.", false },
                    { 2, "Drama", "Emotional and story-driven films.", false },
                    { 3, "Sci-Fi", "Science fiction themes.", false },
                    { 4, "Thriller", "Suspenseful and exciting plots.", false },
                    { 5, "Comedy", "Light-hearted and humorous content.", false }
                });

            // Movies
            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "Name", "Year", "ImdbId", "ImdbRating", "Poster", "DirectorId", "EditedById", "IsDeleted" },
                values: new object[,]
                {
                    { 1, "Inception", 2010, "tt1375666", 8.8, "https://m.media-amazon.com/images/M/MV5BMjAxMzY3NjcxNF5BMl5BanBnXkFtZTcwNTI5OTM0Mw@@._V1_.jpg", 1, 1, false },
                    { 2, "Jurassic Park", 1993, "tt0107290", 8.1, "https://m.media-amazon.com/images/M/MV5BMjM2MDgxMDg0Nl5BMl5BanBnXkFtZTgwNTM2OTM5NDE@._V1_FMjpg_UX1000_.jpg", 2, 1, false },
                    { 3, "Pulp Fiction", 1994, "tt0110912", 8.9, "https://m.media-amazon.com/images/M/MV5BYTViYTE3ZGQtNDBlMC00ZTAyLTkyODMtZGRiZDg0MjA2YThkXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg", 3, 1, false },
                    { 4, "Interstellar", 2014, "tt0816692", 8.6, "https://m.media-amazon.com/images/M/MV5BYzdjMDAxZGItMjI2My00ODA1LTlkNzItOWFjMDU5ZDJlYWY3XkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg", 1, 2, false },
                    { 5, "The Matrix", 1999, "tt0133093", 8.7, "https://m.media-amazon.com/images/M/MV5BN2NmN2VhMTQtMDNiOS00NDlhLTliMjgtODE2ZTY0ODQyNDRhXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg", 1, 2, false }
                });

            // MovieCopies
            migrationBuilder.InsertData(
                table: "MovieCopies",
                columns: new[] { "Id", "MovieId", "SerialNumber", "Description", "IsDeleted" },
                values: new object[,]
                {
                    { 1, 1, "SN001", "Blu-ray copy", false },
                    { 2, 1, "SN002", "DVD copy", false },
                    { 3, 2, "SN003", "Collector's edition", false },
                    { 4, 3, "SN004", "4K UHD", false },
                    { 5, 4, "SN005", "Digital release", false }
                });

            // Rentals
            migrationBuilder.InsertData(
                table: "Rentals",
                columns: new[] { "Id", "Date", "BorrowedToId", "MovieCopyId", "BorrowedById", "IsDeleted" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 1), 2, 1, 1, false },
                    { 2, new DateTime(2025, 7, 5), 3, 3, 1, false },
                    { 3, new DateTime(2025, 7, 10), 4, 4, 2, false }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Rentals");
            migrationBuilder.Sql("DELETE FROM MovieCopies");
            migrationBuilder.Sql("DELETE FROM Movies");
            migrationBuilder.Sql("DELETE FROM Genres");
            migrationBuilder.Sql("DELETE FROM Actors");
            migrationBuilder.Sql("DELETE FROM Directors");
            migrationBuilder.Sql("DELETE FROM Users");
        }
    }
}
