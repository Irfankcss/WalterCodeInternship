<script setup>
import { onMounted } from 'vue'
import $ from 'jquery'
import 'datatables.net-bs4'
onMounted(() => { $('#dataTable').DataTable() })

const movies = [
  {
    imdbID: 'tt0111161',
    imageUrl: 'https://m.media-amazon.com/images/M/MV5BMDFkYTc0MGEtZmNhMC00ZDIzLWFmNTEtODM1ZmRlYWMwMWFmXkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_SX300.jpg',
    title: 'The Shawshank Redemption',
    director: 'Frank Darabont',
    publishedYear: 1994,
    imdbRating: 9.3,
    frontText: 'A banker convicted of uxoricide forms a friendship over a quarter century with a hardened convict.',
    backText: 'Maintaining his innocence and trying to remain hopeful through simple compassion.',
    link: 'https://www.imdb.com/title/tt0111161/',
    linkTitle: 'View on IMDb'
  },
  {
    imdbID: 'tt0068646',
    imageUrl: 'https://preview.redd.it/whats-the-most-famous-movie-youve-never-seen-and-really-v0-rw4x7o2pby5d1.jpeg?auto=webp&s=c94d8dcba68000e75702362e0a05e173933e84b9',
    title: 'The Godfather',
    director: 'Francis Ford Coppola',
    publishedYear: 1972,
    imdbRating: 9.2,
    frontText: 'The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.',
    backText: 'A classic mafia drama about power, family, and betrayal.',
    link: 'https://www.imdb.com/title/tt0068646/',
    linkTitle: 'View on IMDb'
  },
  {
    imdbID: 'tt0071562',
    imageUrl: 'https://m.media-amazon.com/images/I/61AcsPclCXL._UF350,350_QL80_.jpg',
    title: 'The Godfather: Part II',
    director: 'Francis Ford Coppola',
    publishedYear: 1974,
    imdbRating: 9.0,
    frontText: 'The early life and career of Vito Corleone in 1920s New York is portrayed.',
    backText: 'A sequel and prequel to The Godfather, expanding the Corleone saga.',
    link: 'https://www.imdb.com/title/tt0071562/',
    linkTitle: 'View on IMDb'
  },
  {
    imdbID: 'tt0468569',
    imageUrl: 'https://m.media-amazon.com/images/M/MV5BMTMxNTMwODM0NF5BMl5BanBnXkFtZTcwODAyMTk2Mw@@._V1_FMjpg_UX1000_.jpg',
    title: 'The Dark Knight',
    director: 'Christopher Nolan',
    publishedYear: 2008,
    imdbRating: 9.0,
    frontText: 'Batman faces the Joker, a criminal mastermind who plunges Gotham into chaos.',
    backText: 'A gripping superhero thriller with legendary performances.',
    link: 'https://www.imdb.com/title/tt0468569/',
    linkTitle: 'View on IMDb'
  },
  {
    imdbID: 'tt0050083',
    imageUrl: 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS_fgso1s9iRj38tZqt3EHoGIWaEx6FNUaInQ&s',
    title: '12 Angry Men',
    director: 'Sidney Lumet',
    publishedYear: 1957,
    imdbRating: 9.0,
    frontText: 'A jury holdout attempts to prevent a miscarriage of justice.',
    backText: 'A tense courtroom drama about prejudice and integrity.',
    link: 'https://www.imdb.com/title/tt0050083/',
    linkTitle: 'View on IMDb'
  },
  {
    imdbID: 'tt0108052',
    imageUrl: 'https://m.media-amazon.com/images/M/MV5BNjM1ZDQxYWUtMzQyZS00MTE1LWJmZGYtNGUyNTdlYjM3ZmVmXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg',
    title: 'Schindler\'s List',
    director: 'Steven Spielberg',
    publishedYear: 1993,
    imdbRating: 8.9,
    frontText: 'In German-occupied Poland during World War II, Oskar Schindler gradually becomes concerned for his Jewish workforce.',
    backText: 'A powerful story of courage and humanity.',
    link: 'https://www.imdb.com/title/tt0108052/',
    linkTitle: 'View on IMDb'
  },
  {
    imdbID: 'tt0167260',
    imageUrl: 'https://m.media-amazon.com/images/M/MV5BMTZkMjBjNWMtZGI5OC00MGU0LTk4ZTItODg2NWM3NTVmNWQ4XkEyXkFqcGc@._V1_.jpg',
    title: 'The Lord of the Rings: The Return of the King',
    director: 'Peter Jackson',
    publishedYear: 2003,
    imdbRating: 8.9,
    frontText: 'Gandalf and Aragorn lead the World of Men against Sauron\'s army.',
    backText: 'The epic conclusion to the Lord of the Rings trilogy.',
    link: 'https://www.imdb.com/title/tt0167260/',
    linkTitle: 'View on IMDb'
  },
  {
    imdbID: 'tt0110912',
    imageUrl: 'https://m.media-amazon.com/images/M/MV5BYTViYTE3ZGQtNDBlMC00ZTAyLTkyODMtZGRiZDg0MjA2YThkXkEyXkFqcGc@._V1_.jpg',
    title: 'Pulp Fiction',
    director: 'Quentin Tarantino',
    publishedYear: 1994,
    imdbRating: 8.9,
    frontText: 'The lives of two mob hitmen, a boxer, a gangster\'s wife, and a pair of diner bandits intertwine.',
    backText: 'A stylish, nonlinear crime drama.',
    link: 'https://www.imdb.com/title/tt0110912/',
    linkTitle: 'View on IMDb'
  },
  {
    imdbID: 'tt0060196',
    imageUrl: 'https://upload.wikimedia.org/wikipedia/en/4/45/Good_the_bad_and_the_ugly_poster.jpg',
    title: 'The Good, the Bad and the Ugly',
    director: 'Sergio Leone',
    publishedYear: 1966,
    imdbRating: 8.8,
    frontText: 'A bounty hunting scam joins two men in an uneasy alliance against a third in a race to find a fortune in gold.',
    backText: 'A classic spaghetti western.',
    link: 'https://www.imdb.com/title/tt0060196/',
    linkTitle: 'View on IMDb'
  },
  {
    imdbID: 'tt0137523',
    imageUrl: 'https://m.media-amazon.com/images/M/MV5BOTgyOGQ1NDItNGU3Ny00MjU3LTg2YWEtNmEyYjBiMjI1Y2M5XkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg',
    title: 'Fight Club',
    director: 'David Fincher',
    publishedYear: 1999,
    imdbRating: 8.8,
    frontText: 'An insomniac office worker and a devil-may-care soap maker form an underground fight club.',
    backText: 'A dark, twisted look at consumerism and identity.',
    link: 'https://www.imdb.com/title/tt0137523/',
    linkTitle: 'View on IMDb'
  },
  {
    imdbID: 'tt0120737',
    imageUrl: 'https://upload.wikimedia.org/wikipedia/en/f/fb/Lord_Rings_Fellowship_Ring.jpg',
    title: 'The Lord of the Rings: The Fellowship of the Ring',
    director: 'Peter Jackson',
    publishedYear: 2001,
    imdbRating: 8.8,
    frontText: 'A meek Hobbit and eight companions set out on a journey to destroy the One Ring.',
    backText: 'The beginning of an epic adventure.',
    link: 'https://www.imdb.com/title/tt0120737/',
    linkTitle: 'View on IMDb'
  },
  {
    imdbID: 'tt0109830',
    imageUrl: 'https://m.media-amazon.com/images/M/MV5BNDYwNzVjMTItZmU5YS00YjQ5LTljYjgtMjY2NDVmYWMyNWFmXkEyXkFqcGc@._V1_.jpg',
    title: 'Forrest Gump',
    director: 'Robert Zemeckis',
    publishedYear: 1994,
    imdbRating: 8.8,
    frontText: 'The presidencies of Kennedy and Johnson, the Vietnam War, and more through the eyes of Forrest Gump.',
    backText: 'A heartwarming tale of innocence and destiny.',
    link: 'https://www.imdb.com/title/tt0109830/',
    linkTitle: 'View on IMDb'
  },
  {
    imdbID: 'tt1375666',
    imageUrl: 'https://m.media-amazon.com/images/M/MV5BMjAxMzY3NjcxNF5BMl5BanBnXkFtZTcwNTI5OTM0Mw@@._V1_.jpg',
    title: 'Inception',
    director: 'Christopher Nolan',
    publishedYear: 2010,
    imdbRating: 8.8,
    frontText: 'A thief who steals corporate secrets through dream-sharing technology is given the inverse task of planting an idea.',
    backText: 'A mind-bending sci-fi thriller.',
    link: 'https://www.imdb.com/title/tt1375666/',
    linkTitle: 'View on IMDb'
  },
  {
    imdbID: 'tt0167261',
    imageUrl: 'https://m.media-amazon.com/images/M/MV5BMGQxMDdiOWUtYjc1Ni00YzM1LWE2NjMtZTg3Y2JkMjEzMTJjXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg',
    title: 'The Lord of the Rings: The Two Towers',
    director: 'Peter Jackson',
    publishedYear: 2002,
    imdbRating: 8.7,
    frontText: 'While Frodo and Sam edge closer to Mordor, the divided fellowship makes a stand against Sauron\'s new ally.',
    backText: 'The middle chapter of the Lord of the Rings trilogy.',
    link: 'https://www.imdb.com/title/tt0167261/',
    linkTitle: 'View on IMDb'
  },
  {
    imdbID: 'tt0080684',
    imageUrl: 'https://m.media-amazon.com/images/M/MV5BMTkxNGFlNDktZmJkNC00MDdhLTg0MTEtZjZiYWI3MGE5NWIwXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg',
    title: 'Star Wars: Episode V - The Empire Strikes Back',
    director: 'Irvin Kershner',
    publishedYear: 1980,
    imdbRating: 8.7,
    frontText: 'After the Rebels are brutally overpowered by the Empire, Luke Skywalker begins Jedi training.',
    backText: 'The dark and dramatic second act of the original Star Wars trilogy.',
    link: 'https://www.imdb.com/title/tt0080684/',
    linkTitle: 'View on IMDb'
  },
  {
    imdbID: 'tt0133093',
    imageUrl: 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRfSjSWOCaw5dnDL2GT1zFd9RMCgUGw5Q2Cfg&s',
    title: 'The Matrix',
    director: 'Lana Wachowski, Lilly Wachowski',
    publishedYear: 1999,
    imdbRating: 8.7,
    frontText: 'A computer hacker learns about the true nature of his reality and his role in the war against its controllers.',
    backText: 'A revolutionary sci-fi action film.',
    link: 'https://www.imdb.com/title/tt0133093/',
    linkTitle: 'View on IMDb'
  },
  {
    imdbID: 'tt0099685',
    imageUrl: 'https://m.media-amazon.com/images/M/MV5BN2E5NzI2ZGMtY2VjNi00YTRjLWI1MDUtZGY5OWU1MWJjZjRjXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg',
    title: 'Goodfellas',
    director: 'Martin Scorsese',
    publishedYear: 1990,
    imdbRating: 8.7,
    frontText: 'The story of Henry Hill and his life in the mob.',
    backText: 'A gritty, stylish gangster classic.',
    link: 'https://www.imdb.com/title/tt0099685/',
    linkTitle: 'View on IMDb'
  },
  {
    imdbID: 'tt0073486',
    imageUrl: 'https://m.media-amazon.com/images/M/MV5BYjBkMjgzMzYtNzRiMS00NDc3LWE4YTUtZjYxYjZhNjNhYzhhXkEyXkFqcGc@._V1_.jpg',
    title: 'One Flew Over the Cuckoo\'s Nest',
    director: 'Milos Forman',
    publishedYear: 1975,
    imdbRating: 8.7,
    frontText: 'A criminal pleads insanity and is admitted to a mental institution.',
    backText: 'A powerful drama about freedom and authority.',
    link: 'https://www.imdb.com/title/tt0073486/',
    linkTitle: 'View on IMDb'
  },
  {
    imdbID: 'tt0047478',
    imageUrl: 'https://upload.wikimedia.org/wikipedia/commons/thumb/b/ba/Seven_Samurai_poster.jpg/1200px-Seven_Samurai_poster.jpg',
    title: 'Seven Samurai',
    director: 'Akira Kurosawa',
    publishedYear: 1954,
    imdbRating: 8.6,
    frontText: 'A poor village hires seven samurai to defend them from bandits.',
    backText: 'A legendary Japanese epic.',
    link: 'https://www.imdb.com/title/tt0047478/',
    linkTitle: 'View on IMDb'
  }
]
</script>

<template>
  <div class="container-fluid">
    <h1 class="h3 mb-2 text-gray-800">Movies Table</h1>
    <div class="card shadow mb-4">
      <div class="card-header py-3">
        <h6 class="m-0 font-weight-bold text-primary">Movie List</h6>
      </div>
      <div class="card-body">
        <div class="table-responsive">
          <table class="table table-bordered" id="dataTable" width="100%" cellspacing="0">
            <thead>
              <tr>
                <th>Title</th>
                <th>Director</th>
                <th>Year</th>
                <th>IMDb Rating</th>
                <th>IMDb Link</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="movie in movies" :key="movie.imdbID">
                <td>{{ movie.title }}</td>
                <td>{{ movie.director }}</td>
                <td>{{ movie.publishedYear }}</td>
                <td>{{ movie.imdbRating }}</td>
                <td>
                  <a :href="movie.link" target="_blank" rel="noopener">IMDb</a>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>
