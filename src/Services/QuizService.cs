using TomAndJerry.Model;

namespace TomAndJerry.Services
{
    public class QuizService : IQuizService
    {
        private readonly List<QuizQuestion> _questions;

        public QuizService()
        {
            _questions = InitializeQuestions();
        }

        public async Task<List<QuizQuestion>> GetQuizQuestionsAsync(int count = 10, string difficulty = "all")
        {
            await Task.Delay(100); // Simulate async operation

            var filteredQuestions = difficulty == "all" 
                ? _questions 
                : _questions.Where(q => q.Difficulty.Equals(difficulty, StringComparison.OrdinalIgnoreCase)).ToList();

            var random = new Random();
            return filteredQuestions.OrderBy(x => random.Next()).Take(count).ToList();
        }

        public async Task<QuizQuestion?> GetQuestionByIdAsync(int id)
        {
            await Task.Delay(50);
            return _questions.FirstOrDefault(q => q.Id == id);
        }

        public async Task<QuizResult> CalculateResultAsync(List<QuizAnswer> answers, TimeSpan timeTaken)
        {
            await Task.Delay(100);

            var correctAnswers = answers.Count(a => a.IsCorrect);
            var wrongAnswers = answers.Count - correctAnswers;
            var scorePercentage = answers.Count > 0 ? (double)correctAnswers / answers.Count * 100 : 0;

            var grade = GetGrade(scorePercentage);
            var message = GetGradeMessage(grade, scorePercentage);

            return new QuizResult
            {
                TotalQuestions = answers.Count,
                CorrectAnswers = correctAnswers,
                WrongAnswers = wrongAnswers,
                ScorePercentage = scorePercentage,
                TimeTaken = timeTaken,
                Answers = answers,
                Grade = grade,
                Message = message
            };
        }

        public async Task<List<QuizQuestion>> GetQuestionsByCategoryAsync(string category, int count = 10)
        {
            await Task.Delay(100);
            var filteredQuestions = _questions.Where(q => q.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
            var random = new Random();
            return filteredQuestions.OrderBy(x => random.Next()).Take(count).ToList();
        }

        public async Task<List<string>> GetAvailableCategoriesAsync()
        {
            await Task.Delay(50);
            return _questions.Select(q => q.Category).Distinct().OrderBy(c => c).ToList();
        }

        public async Task<List<string>> GetAvailableDifficultiesAsync()
        {
            await Task.Delay(50);
            return _questions.Select(q => q.Difficulty).Distinct().OrderBy(d => d).ToList();
        }

        private string GetGrade(double scorePercentage)
        {
            return scorePercentage switch
            {
                >= 90 => "A+",
                >= 80 => "A",
                >= 70 => "B",
                >= 60 => "C",
                >= 50 => "D",
                _ => "F"
            };
        }

        private string GetGradeMessage(string grade, double scorePercentage)
        {
            return grade switch
            {
                "A+" => "Outstanding! You're a true Tom & Jerry expert! 🏆",
                "A" => "Excellent! You know your Tom & Jerry very well! 🌟",
                "B" => "Great job! You have good knowledge of Tom & Jerry! 👍",
                "C" => "Not bad! You know some Tom & Jerry facts! 😊",
                "D" => "Keep watching! You'll learn more Tom & Jerry! 📺",
                _ => "Time to binge-watch some Tom & Jerry episodes! 🎬"
            };
        }

        private List<QuizQuestion> InitializeQuestions()
        {
            var questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Id = 1,
                    Question = "What year did the first Tom and Jerry cartoon premiere?",
                    Options = new List<string> { "1939", "1940", "1941", "1942" },
                    CorrectAnswerIndex = 1,
                    Explanation = "The first Tom and Jerry cartoon, 'Puss Gets the Boot', premiered in 1940.",
                    Category = "history",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 2,
                    Question = "What is Tom's full name?",
                    Options = new List<string> { "Thomas Cat", "Tom Cat", "Thomas", "Tom" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Tom's full name is Tom Cat, though he's often just called Tom.",
                    Category = "characters",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 3,
                    Question = "How many Academy Awards did Tom and Jerry win?",
                    Options = new List<string> { "5", "6", "7", "8" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Tom and Jerry won 7 Academy Awards for Best Animated Short Film.",
                    Category = "awards",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 4,
                    Question = "What is Jerry's favorite food?",
                    Options = new List<string> { "Crackers", "Cheese", "Bread", "Nuts" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Jerry's favorite food is cheese, which often gets him into trouble with Tom.",
                    Category = "characters",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 5,
                    Question = "Who created Tom and Jerry?",
                    Options = new List<string> { "Walt Disney", "William Hanna and Joseph Barbera", "Chuck Jones", "Tex Avery" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Tom and Jerry were created by William Hanna and Joseph Barbera at MGM.",
                    Category = "history",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 6,
                    Question = "What color is Tom?",
                    Options = new List<string> { "Black", "Orange", "Blue-grey", "White" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Tom is typically blue-grey in color, though this can vary in different episodes.",
                    Category = "characters",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 7,
                    Question = "In which decade did Tom and Jerry cartoons stop being produced by MGM?",
                    Options = new List<string> { "1950s", "1960s", "1970s", "1980s" },
                    CorrectAnswerIndex = 1,
                    Explanation = "MGM stopped producing Tom and Jerry cartoons in the 1960s, specifically in 1958.",
                    Category = "history",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 8,
                    Question = "What is the name of Tom and Jerry's owner?",
                    Options = new List<string> { "Mammy Two Shoes", "Spike", "Tyke", "They don't have an owner" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Mammy Two Shoes was Tom and Jerry's owner in the early cartoons, though she was later replaced.",
                    Category = "characters",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 9,
                    Question = "Which character is Tom's rival for Jerry's attention?",
                    Options = new List<string> { "Spike", "Butch", "Tuffy", "Nibbles" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Butch is another cat who often competes with Tom for Jerry's attention.",
                    Category = "characters",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 10,
                    Question = "What is the name of Jerry's nephew?",
                    Options = new List<string> { "Tuffy", "Nibbles", "Jerry Jr.", "Little Jerry" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Nibbles is Jerry's nephew who often appears in the cartoons.",
                    Category = "characters",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 11,
                    Question = "Which studio originally produced Tom and Jerry?",
                    Options = new List<string> { "Warner Bros.", "Disney", "MGM", "Paramount" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Tom and Jerry were originally produced by MGM (Metro-Goldwyn-Mayer).",
                    Category = "history",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 12,
                    Question = "What is the typical length of a Tom and Jerry cartoon?",
                    Options = new List<string> { "5-7 minutes", "10-15 minutes", "20-30 minutes", "45-60 minutes" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Most Tom and Jerry cartoons are short films, typically 5-7 minutes long.",
                    Category = "general",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 13,
                    Question = "Which character is known for being a bulldog?",
                    Options = new List<string> { "Spike", "Butch", "Tom", "Jerry" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Spike is the bulldog character who often protects Jerry from Tom.",
                    Category = "characters",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 14,
                    Question = "What is the name of Spike's son?",
                    Options = new List<string> { "Tyke", "Tuffy", "Nibbles", "Spike Jr." },
                    CorrectAnswerIndex = 0,
                    Explanation = "Tyke is Spike's son who often appears with his father.",
                    Category = "characters",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 15,
                    Question = "In which cartoon did Tom and Jerry first appear?",
                    Options = new List<string> { "Puss Gets the Boot", "The Midnight Snack", "Fraidy Cat", "Mouse Trouble" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Tom and Jerry first appeared in 'Puss Gets the Boot' in 1940.",
                    Category = "history",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 16,
                    Question = "What type of animal is Jerry?",
                    Options = new List<string> { "Rat", "Mouse", "Hamster", "Gerbil" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Jerry is a mouse, which is why Tom (the cat) is always trying to catch him.",
                    Category = "characters",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 17,
                    Question = "Which decade saw the revival of Tom and Jerry cartoons?",
                    Options = new List<string> { "1970s", "1980s", "1990s", "2000s" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Tom and Jerry cartoons were revived in the 1990s with new episodes and movies.",
                    Category = "history",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 18,
                    Question = "What is the name of the character who is a little mouse and Jerry's friend?",
                    Options = new List<string> { "Tuffy", "Nibbles", "Little Jerry", "Mouse" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Tuffy is a little mouse who is often Jerry's friend and companion.",
                    Category = "characters",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 19,
                    Question = "Which Tom and Jerry cartoon won the first Academy Award?",
                    Options = new List<string> { "The Yankee Doodle Mouse", "Mouse Trouble", "Quiet Please!", "The Cat Concerto" },
                    CorrectAnswerIndex = 0,
                    Explanation = "'The Yankee Doodle Mouse' won the first Academy Award for Tom and Jerry in 1943.",
                    Category = "awards",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 20,
                    Question = "What is the typical relationship between Tom and Jerry?",
                    Options = new List<string> { "Best friends", "Enemies", "Brothers", "Neighbors" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Tom and Jerry are typically enemies, with Tom always trying to catch Jerry.",
                    Category = "general",
                    Difficulty = "easy"
                }
                ,
                // Added authentic history questions to reach 20 total
                new QuizQuestion
                {
                    Id = 21,
                    Question = "In what year did MGM halt the original Hanna-Barbera Tom and Jerry shorts?",
                    Options = new List<string> { "1955", "1957", "1958", "1960" },
                    CorrectAnswerIndex = 2,
                    Explanation = "MGM shut its animation unit in 1958, ending the original Hanna-Barbera run.",
                    Category = "history",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 22,
                    Question = "Where were Gene Deitch's Tom and Jerry shorts produced in the early 1960s?",
                    Options = new List<string> { "Los Angeles, USA", "London, UK", "Prague, Czechoslovakia", "Tokyo, Japan" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Gene Deitch directed shorts in Prague for Rembrandt Films during 1961–1962.",
                    Category = "history",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 23,
                    Question = "Who took over Tom and Jerry with a new style from 1963 to 1967?",
                    Options = new List<string> { "Tex Avery", "Gene Deitch", "Chuck Jones", "Friz Freleng" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Chuck Jones produced a new series at Sib Tower 12/MGM Animation from 1963–1967.",
                    Category = "history",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 24,
                    Question = "What was the last Hanna–Barbera theatrical short before MGM closed the unit?",
                    Options = new List<string> { "Down Beat Bear", "Tot Watchers", "Blue Cat Blues", "The Truce Hurts" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Tot Watchers (1958) was the final Hanna–Barbera Tom and Jerry theatrical short.",
                    Category = "history",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 25,
                    Question = "What were Tom and Jerry originally called in their first appearance?",
                    Options = new List<string> { "Tom and Jerry", "Jasper and Jinx", "Thomas and Jerome", "Cat and Mouse" },
                    CorrectAnswerIndex = 1,
                    Explanation = "In 'Puss Gets the Boot' (1940), the characters were called Jasper (Tom) and Jinx (Jerry).",
                    Category = "history",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 26,
                    Question = "Which company financed the early 1960s Tom and Jerry shorts directed by Gene Deitch?",
                    Options = new List<string> { "Sib Tower 12", "Rembrandt Films", "Warner Bros.", "DePatie–Freleng" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Rembrandt Films produced the Gene Deitch era shorts in Prague.",
                    Category = "history",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 27,
                    Question = "Who composed much of the music for the classic Tom and Jerry shorts at MGM?",
                    Options = new List<string> { "Carl Stalling", "Scott Bradley", "Henry Mancini", "Max Steiner" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Scott Bradley's scores are a hallmark of the classic MGM shorts.",
                    Category = "history",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 28,
                    Question = "Which widescreen process was used in some 1950s Tom and Jerry shorts?",
                    Options = new List<string> { "VistaVision", "CinemaScope", "Cinerama", "Panavision 70" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Several mid-1950s shorts were produced in CinemaScope.",
                    Category = "history",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 29,
                    Question = "Which production unit created the 1963–1967 Tom and Jerry series with Chuck Jones?",
                    Options = new List<string> { "DePatie–Freleng", "Filmation", "Sib Tower 12/MGM Animation", "Hanna-Barbera Productions" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Chuck Jones produced them at Sib Tower 12, later MGM Animation/Visual Arts.",
                    Category = "history",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 30,
                    Question = "Which short is widely considered the first Tom and Jerry cartoon?",
                    Options = new List<string> { "Puss Gets the Boot", "The Midnight Snack", "The Night Before Christmas", "Mouse Trouble" },
                    CorrectAnswerIndex = 0,
                    Explanation = "'Puss Gets the Boot' (1940) introduced the cat-and-mouse duo to audiences.",
                    Category = "history",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 31,
                    Question = "Which creators developed Tom and Jerry while working at MGM?",
                    Options = new List<string> { "Tex Avery and Friz Freleng", "William Hanna and Joseph Barbera", "Chuck Jones and Friz Freleng", "Bob Clampett and Frank Tashlin" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Hanna and Barbera created Tom and Jerry at MGM in 1940.",
                    Category = "history",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 32,
                    Question = "Which decade introduced the TV series 'The Tom and Jerry Show' with toned-down violence?",
                    Options = new List<string> { "1950s", "1960s", "1970s", "1980s" },
                    CorrectAnswerIndex = 2,
                    Explanation = "The 1975 TV series by Hanna-Barbera adapted the characters for television standards.",
                    Category = "history",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 33,
                    Question = "Which early short was among the first Christmas-themed Tom and Jerry cartoons?",
                    Options = new List<string> { "The Night Before Christmas", "The Cat Concerto", "Salt Water Tabby", "Solid Serenade" },
                    CorrectAnswerIndex = 0,
                    Explanation = "'The Night Before Christmas' (1941) was an early holiday-themed entry.",
                    Category = "history",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 34,
                    Question = "Which short marked Tom and Jerry's 1950s era moving into widescreen for some releases?",
                    Options = new List<string> { "Pet Peeve", "Touché, Pussy Cat!", "Southbound Duckling", "Pet Peeve (CinemaScope)" },
                    CorrectAnswerIndex = 0,
                    Explanation = "'Pet Peeve' (1954) was one of the mid-1950s entries as formats evolved.",
                    Category = "history",
                    Difficulty = "hard"
                },
                // Added authentic characters questions to reach 20 total for characters
                new QuizQuestion
                {
                    Id = 35,
                    Question = "What is the bulldog's name who often thwarts Tom?",
                    Options = new List<string> { "Tyke", "Spike", "Butch", "Droopy" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Spike is the tough bulldog who protects Jerry and his son Tyke.",
                    Category = "characters",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 36,
                    Question = "What is the name of Spike's puppy?",
                    Options = new List<string> { "Max", "Tyke", "Rex", "Buddy" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Tyke is Spike's adorable and frequently featured pup.",
                    Category = "characters",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 37,
                    Question = "Which alley cat frequently rivals Tom and sometimes courts Toodles?",
                    Options = new List<string> { "Butch", "Top Cat", "Sylvester", "Claude" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Butch is the black alley cat who competes with Tom.",
                    Category = "characters",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 38,
                    Question = "What is the name of the white female cat Tom often admires?",
                    Options = new List<string> { "Toodles Galore", "Fluffy", "Kitty White", "Lucille" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Toodles Galore appears as Tom's glamorous love interest in several shorts.",
                    Category = "characters",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 39,
                    Question = "Which tiny grey mouse is Jerry's young companion in multiple episodes?",
                    Options = new List<string> { "Nibbles/Tuffy", "Morty", "Mighty Mouse", "Pixie" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Nibbles, also known as Tuffy, is Jerry's small, hungry sidekick.",
                    Category = "characters",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 40,
                    Question = "What is the name of the canary that sometimes teams up with Jerry?",
                    Options = new List<string> { "Tweety", "Quacker", "Cuckoo", "Cuckoo Canary" },
                    CorrectAnswerIndex = 3,
                    Explanation = "Cuckoo Canary appears in shorts where Tom faces off against a feisty bird.",
                    Category = "characters",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 41,
                    Question = "Who is the duckling character often seen causing chaos around Tom?",
                    Options = new List<string> { "Daffy", "Donald", "Quacker", "Little Quack" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Quacker is the naive little duckling frequently rescued by Jerry.",
                    Category = "characters",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 42,
                    Question = "Which character is a muscular black cat sometimes seen intimidating Tom?",
                    Options = new List<string> { "Meathead", "Butch", "Lightning", "Muscles" },
                    CorrectAnswerIndex = 3,
                    Explanation = "Muscles is Jerry's strong cousin who helps him overpower Tom.",
                    Category = "characters",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 43,
                    Question = "What is the name of the housemaid character from early cartoons often scolding Tom?",
                    Options = new List<string> { "Mammy Two Shoes", "Aunt Jemima", "Mama Bear", "Mrs. Two Shoes" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Mammy Two Shoes is a recurring human character in early MGM shorts.",
                    Category = "characters",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 44,
                    Question = "Which mouse relative appears dressed as a baby wearing a diaper?",
                    Options = new List<string> { "Nibbles/Tuffy", "Little Jerry", "Pip", "Mini Mouse" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Nibbles (Tuffy) is depicted as a diapered baby mouse in many appearances.",
                    Category = "characters",
                    Difficulty = "easy"
                },
                // Added authentic awards questions to reach 20 total for awards
                new QuizQuestion
                {
                    Id = 45,
                    Question = "How many Academy Awards did Tom and Jerry win for Best Animated Short?",
                    Options = new List<string> { "5", "6", "7", "8" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Tom and Jerry won 7 Oscars in the Best Animated Short Film category.",
                    Category = "awards",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 46,
                    Question = "Which Tom and Jerry short won their first Academy Award?",
                    Options = new List<string> { "Mouse Trouble", "The Cat Concerto", "The Yankee Doodle Mouse", "Quiet Please!" },
                    CorrectAnswerIndex = 2,
                    Explanation = "The Yankee Doodle Mouse won in 1943 (awarded 1944).",
                    Category = "awards",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 47,
                    Question = "Which year did 'The Cat Concerto' win the Academy Award?",
                    Options = new List<string> { "1943", "1947", "1951", "1953" },
                    CorrectAnswerIndex = 1,
                    Explanation = "The Cat Concerto won the Oscar awarded in 1947 for 1946 releases.",
                    Category = "awards",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 48,
                    Question = "Which short beat a Walt Disney film to win an Oscar for Tom and Jerry?",
                    Options = new List<string> { "The Little Orphan", "Mouse Cleaning", "Nit-Witty Kitty", "Casanova Cat" },
                    CorrectAnswerIndex = 0,
                    Explanation = "The Little Orphan (1949) triumphed over Disney competition at the Oscars.",
                    Category = "awards",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 49,
                    Question = "Which short earned an Oscar for featuring Tom as a concert pianist?",
                    Options = new List<string> { "The Cat Concerto", "The Two Mouseketeers", "Johann Mouse", "Heavenly Puss" },
                    CorrectAnswerIndex = 0,
                    Explanation = "The Cat Concerto showcased Tom at the piano performing Liszt.",
                    Category = "awards",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 50,
                    Question = "Which Tom and Jerry short won the Oscar awarded in 1953?",
                    Options = new List<string> { "Little School Mouse", "Two Little Indians", "Johann Mouse", "The Missing Mouse" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Johann Mouse (1952) received the Academy Award in 1953.",
                    Category = "awards",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 51,
                    Question = "How many Oscar nominations did Tom and Jerry receive overall (wins included)?",
                    Options = new List<string> { "7", "10", "13", "15" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Tom and Jerry amassed 13 nominations, winning 7 of them.",
                    Category = "awards",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 52,
                    Question = "Which short featuring musketeer costumes won an Academy Award?",
                    Options = new List<string> { "Touché, Pussy Cat!", "The Two Mouseketeers", "The Mouse Comes to Dinner", "Jerry and Jumbo" },
                    CorrectAnswerIndex = 1,
                    Explanation = "The Two Mouseketeers (1952) won the Oscar awarded in 1953.",
                    Category = "awards",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 53,
                    Question = "Which studio accepted the Oscars for Tom and Jerry's classic wins?",
                    Options = new List<string> { "Warner Bros.", "MGM", "Hanna-Barbera Productions", "Universal" },
                    CorrectAnswerIndex = 1,
                    Explanation = "The classic shorts were produced at MGM during the Oscar-winning era.",
                    Category = "awards",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 54,
                    Question = "Which short won the Academy Award the same year as Disney's 'Mickey and the Seal' was nominated?",
                    Options = new List<string> { "Mouse Trouble", "The Little Orphan", "Quiet Please!", "The Zoot Cat" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Quiet Please! (1945) won against strong competition, including Disney nominees.",
                    Category = "awards",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 55,
                    Question = "Which Tom and Jerry short won the Oscar awarded in 1946?",
                    Options = new List<string> { "Mouse Trouble", "The Truce Hurts", "Solid Serenade", "Trap Happy" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Mouse Trouble won the Academy Award awarded in 1946 for 1944 releases.",
                    Category = "awards",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 56,
                    Question = "Which short won the Oscar for its inventive use of a conductor's baton?",
                    Options = new List<string> { "Johann Mouse", "The Cat Concerto", "Kitty Foiled", "Cue Ball Cat" },
                    CorrectAnswerIndex = 1,
                    Explanation = "The Cat Concerto is renowned for Tom's performance at the piano and baton gags.",
                    Category = "awards",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 57,
                    Question = "Which Oscar-winning short features Jerry assisting a famous composer on screen?",
                    Options = new List<string> { "Johann Mouse", "Dr. Jekyll and Mr. Mouse", "Jerry and the Goldfish", "The Little Orphan" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Johann Mouse has Jerry inspiring the music of Johann Strauss Jr.",
                    Category = "awards",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 58,
                    Question = "Which short earned Tom and Jerry an Oscar during World War II era releases?",
                    Options = new List<string> { "Fine Feathered Friend", "The Yankee Doodle Mouse", "The Mouse Comes to Dinner", "Baby Puss" },
                    CorrectAnswerIndex = 1,
                    Explanation = "The Yankee Doodle Mouse (1943) is a wartime-themed short that won the Oscar.",
                    Category = "awards",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 59,
                    Question = "Which of these did NOT win the Oscar but was nominated?",
                    Options = new List<string> { "Mouse Trouble", "Dr. Jekyll and Mr. Mouse", "The Little Orphan", "Quiet Please!" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Dr. Jekyll and Mr. Mouse (1947) was nominated but did not win.",
                    Category = "awards",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 60,
                    Question = "Which creators received Oscars for Tom and Jerry shorts during the 1940s?",
                    Options = new List<string> { "Tex Avery and Friz Freleng", "William Hanna and Joseph Barbera", "Chuck Jones and Friz Freleng", "Gene Deitch and Bill Snyder" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Hanna and Barbera's MGM shorts brought home multiple Academy Awards.",
                    Category = "awards",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 61,
                    Question = "Which Oscar-winning short features a Thanksgiving dinner plotline?",
                    Options = new List<string> { "The Little Orphan", "The Cat Concerto", "Johann Mouse", "Heavenly Puss" },
                    CorrectAnswerIndex = 0,
                    Explanation = "The Little Orphan (1949) centers on a Thanksgiving feast with Nibbles.",
                    Category = "awards",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 62,
                    Question = "Which Tom and Jerry short won the Oscar awarded in 1952?",
                    Options = new List<string> { "Saturday Evening Puss", "The Two Mouseketeers", "Texas Tom", "Triplet Trouble" },
                    CorrectAnswerIndex = 1,
                    Explanation = "The Two Mouseketeers won the Academy Award awarded in 1952 for 1951 releases.",
                    Category = "awards",
                    Difficulty = "hard"
                },
                // Added authentic general questions to reach 20+ total for general
                new QuizQuestion
                {
                    Id = 63,
                    Question = "What kind of animal is Tom?",
                    Options = new List<string> { "Dog", "Cat", "Mouse", "Rabbit" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Tom is a house cat perpetually chasing Jerry the mouse.",
                    Category = "general",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 64,
                    Question = "What kind of animal is Jerry?",
                    Options = new List<string> { "Hamster", "Rat", "Mouse", "Squirrel" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Jerry is a clever mouse who outsmarts Tom.",
                    Category = "general",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 65,
                    Question = "Which setting is most common in Tom and Jerry cartoons?",
                    Options = new List<string> { "Outer space", "A suburban house", "A pirate ship", "A jungle" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Many shorts take place in and around a suburban home.",
                    Category = "general",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 66,
                    Question = "What is Tom usually trying to do to Jerry?",
                    Options = new List<string> { "Make friends", "Catch him", "Teach him", "Avoid him" },
                    CorrectAnswerIndex = 1,
                    Explanation = "The classic chase revolves around Tom attempting to catch Jerry.",
                    Category = "general",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 67,
                    Question = "Which household item often becomes a comedic weapon?",
                    Options = new List<string> { "Feather duster", "Toaster", "Frying pan", "Vacuum bag" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Slapstick gags frequently involve frying pans and kitchenware.",
                    Category = "general",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 68,
                    Question = "Which best describes the humor style of Tom and Jerry?",
                    Options = new List<string> { "Wordplay", "Silent slapstick", "Political satire", "Romantic comedy" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Visual gags and physical comedy drive the humor.",
                    Category = "general",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 69,
                    Question = "Which musical element is prominent in many shorts?",
                    Options = new List<string> { "Rock anthems", "Orchestral scoring", "Country ballads", "Free-form jazz only" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Orchestral scores closely follow and punctuate the on-screen action.",
                    Category = "general",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 70,
                    Question = "What is a frequent outcome of Tom's plans?",
                    Options = new List<string> { "They work flawlessly", "They backfire comically", "Jerry is captured permanently", "They are never shown" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Tom's traps usually backfire in humorous ways.",
                    Category = "general",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 71,
                    Question = "Which of these is NOT a recurring character?",
                    Options = new List<string> { "Spike", "Tyke", "Butch", "Bugs Bunny" },
                    CorrectAnswerIndex = 3,
                    Explanation = "Bugs Bunny is from the Looney Tunes franchise.",
                    Category = "general",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 72,
                    Question = "What type of storytelling is most common in Tom and Jerry?",
                    Options = new List<string> { "Serialized drama", "Episodic shorts", "Long-form arcs", "Documentary" },
                    CorrectAnswerIndex = 1,
                    Explanation = "They are primarily standalone animated shorts.",
                    Category = "general",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 73,
                    Question = "Which character sometimes protects Jerry from Tom?",
                    Options = new List<string> { "Spike", "Butch", "Mammy Two Shoes", "Quacker" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Spike often intervenes, usually to Tom's detriment.",
                    Category = "general",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 74,
                    Question = "Which phrase best captures the Tom and Jerry dynamic?",
                    Options = new List<string> { "Teacher and student", "Cat and mouse", "Detective and thief", "Doctor and patient" },
                    CorrectAnswerIndex = 1,
                    Explanation = "The archetypal cat-and-mouse chase is the core premise.",
                    Category = "general",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 75,
                    Question = "What is a common resolution after a conflict in some episodes?",
                    Options = new List<string> { "Lasting friendship", "Temporary truce", "Court trial", "Permanent exile" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Occasional truces occur, though rivalry resumes.",
                    Category = "general",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 76,
                    Question = "Which household appliance is often part of the gags?",
                    Options = new List<string> { "Microwave", "Vacuum cleaner", "Dishwasher", "Air fryer" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Vacuum cleaners are frequently used in slapstick routines.",
                    Category = "general",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 77,
                    Question = "Which word describes the typical dialogue in classic shorts?",
                    Options = new List<string> { "Dialogue-heavy", "Narrated", "Mostly silent", "Musical-only with no effects" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Classic shorts rely on minimal dialogue and expressive animation.",
                    Category = "general",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 78,
                    Question = "Which best describes Jerry's personality?",
                    Options = new List<string> { "Timid and fearful", "Clever and resourceful", "Loud and boastful", "Clumsy and shy" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Jerry wins with cleverness rather than strength.",
                    Category = "general",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 79,
                    Question = "Which best describes Tom's approach to catching Jerry?",
                    Options = new List<string> { "Patient planning", "Elaborate traps and schemes", "Diplomatic negotiation", "Ignoring Jerry" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Tom employs complicated traps that often foil himself.",
                    Category = "general",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 80,
                    Question = "Which musical instrument has Tom famously played in a celebrated short?",
                    Options = new List<string> { "Violin", "Piano", "Guitar", "Drums" },
                    CorrectAnswerIndex = 1,
                    Explanation = "In 'The Cat Concerto', Tom plays the piano in a famous performance.",
                    Category = "general",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 81,
                    Question = "Which is a frequent motif of the chase sequences?",
                    Options = new List<string> { "Maze-like house corridors", "City traffic", "Open deserts", "Underwater caves" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Chases through winding hallways are a recurring visual gag.",
                    Category = "general",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 82,
                    Question = "Which best describes the outcome of most episodes?",
                    Options = new List<string> { "Permanent victory for Tom", "Permanent victory for Jerry", "Status quo restored", "Tom leaves the house forever" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Episodes typically reset to the familiar rivalry by the end.",
                    Category = "general",
                    Difficulty = "easy"
                },
                // Five more questions per category (characters, history, awards, general)
                // Characters (83-87)
                new QuizQuestion
                {
                    Id = 83,
                    Question = "What is the name of Jerry's strong cousin who intimidates Tom?",
                    Options = new List<string> { "Butch", "Muscles", "Lightning", "Meathead" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Muscles is the muscular mouse who easily overpowers Tom.",
                    Category = "characters",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 84,
                    Question = "Which cat often competes with Tom for food and attention?",
                    Options = new List<string> { "Butch", "Sylvester", "Top Cat", "Felix" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Butch is Tom's recurring alley-cat rival.",
                    Category = "characters",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 85,
                    Question = "What is the name of the grey kitten occasionally mentored by Tom?",
                    Options = new List<string> { "Lightning", "Tyke", "Topsy", "Nibbles" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Topsy is a mischievous kitten who appears in a few shorts.",
                    Category = "characters",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 86,
                    Question = "Who is the elegant white cat that Tom often tries to impress?",
                    Options = new List<string> { "Toodles Galore", "Kitty White", "Snowball", "Duchess" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Toodles Galore is featured as Tom's love interest.",
                    Category = "characters",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 87,
                    Question = "What is the name of the black-and-white house cat sometimes seen with Tom?",
                    Options = new List<string> { "Lightning", "Meathead", "Butch", "Topsy" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Lightning appears as a rival house cat in certain shorts.",
                    Category = "characters",
                    Difficulty = "hard"
                },
                // History (88-92)
                new QuizQuestion
                {
                    Id = 88,
                    Question = "Which duo created Tom and Jerry at MGM in 1940?",
                    Options = new List<string> { "Tex Avery & Friz Freleng", "William Hanna & Joseph Barbera", "Chuck Jones & Friz Freleng", "Bob Clampett & Frank Tashlin" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Hanna and Barbera originated the series at MGM.",
                    Category = "history",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 89,
                    Question = "Which era followed the original MGM shutdown in 1958?",
                    Options = new List<string> { "Gene Deitch shorts", "Hanna-Barbera TV series", "DePatie–Freleng revival", "Filmation specials" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Gene Deitch produced Prague-made shorts in the early 1960s.",
                    Category = "history",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 90,
                    Question = "Which studio unit produced the 1963–1967 Tom and Jerry series?",
                    Options = new List<string> { "Sib Tower 12/MGM Animation", "Warner Bros. Cartoons", "Famous Studios", "UPA" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Chuck Jones led production at Sib Tower 12/MGM Animation.",
                    Category = "history",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 91,
                    Question = "What was the title of Tom and Jerry's 1941 Christmas short?",
                    Options = new List<string> { "The Night Before Christmas", "A Mouse in the House", "Snowbound Puss", "Holiday Havoc" },
                    CorrectAnswerIndex = 0,
                    Explanation = "An early seasonal entry was 'The Night Before Christmas' (1941).",
                    Category = "history",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 92,
                    Question = "Which 1950s innovation saw some shorts produced in widescreen?",
                    Options = new List<string> { "VistaVision", "CinemaScope", "IMAX", "Technirama" },
                    CorrectAnswerIndex = 1,
                    Explanation = "A few mid-1950s entries were made in CinemaScope.",
                    Category = "history",
                    Difficulty = "hard"
                },
                // Awards (93-97)
                new QuizQuestion
                {
                    Id = 93,
                    Question = "Which Oscar-winning short features a war-themed montage?",
                    Options = new List<string> { "The Yankee Doodle Mouse", "Mouse Trouble", "Quiet Please!", "The Little Orphan" },
                    CorrectAnswerIndex = 0,
                    Explanation = "The wartime short 'The Yankee Doodle Mouse' won an Academy Award.",
                    Category = "awards",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 94,
                    Question = "How many Oscars did Tom and Jerry win during the 1940s?",
                    Options = new List<string> { "2", "3", "5", "7" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Most of their wins came in the 1940s, totaling five.",
                    Category = "awards",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 95,
                    Question = "Which short about Thanksgiving won an Academy Award?",
                    Options = new List<string> { "The Little Orphan", "Dr. Jekyll and Mr. Mouse", "Cat Fishin'", "Triplet Trouble" },
                    CorrectAnswerIndex = 0,
                    Explanation = "'The Little Orphan' (1949) won, featuring a feast with Nibbles.",
                    Category = "awards",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 96,
                    Question = "Which creators accepted multiple Oscars for Tom and Jerry shorts?",
                    Options = new List<string> { "Gene Deitch & Bill Snyder", "William Hanna & Joseph Barbera", "Chuck Jones & Michael Maltese", "Tex Avery & Heck Allen" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Hanna and Barbera's MGM shorts garnered many awards.",
                    Category = "awards",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 97,
                    Question = "Which Oscar-winning short stars Tom as a concert pianist?",
                    Options = new List<string> { "Mouse in Manhattan", "The Cat Concerto", "Salt Water Tabby", "Cue Ball Cat" },
                    CorrectAnswerIndex = 1,
                    Explanation = "'The Cat Concerto' is famous for its piano performance.",
                    Category = "awards",
                    Difficulty = "easy"
                },
                // General (98-102)
                new QuizQuestion
                {
                    Id = 98,
                    Question = "Which best describes the tone of Tom and Jerry cartoons?",
                    Options = new List<string> { "Dark and tragic", "Light, comedic, and energetic", "Romantic and sentimental", "Documentary realism" },
                    CorrectAnswerIndex = 1,
                    Explanation = "They are playful and energetic, driven by visual comedy.",
                    Category = "general",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 99,
                    Question = "Which household room hosts many chases?",
                    Options = new List<string> { "Kitchen", "Attic", "Basement", "Garage" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Kitchen props enable many classic slapstick gags.",
                    Category = "general",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 100,
                    Question = "Which item often becomes a trap in Tom's schemes?",
                    Options = new List<string> { "Books", "String and mousetraps", "Curtains", "Shoes" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Elaborate string-and-trap contraptions are a staple.",
                    Category = "general",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 101,
                    Question = "Which attribute best describes Jerry's advantage over Tom?",
                    Options = new List<string> { "Greater strength", "Greater speed", "Greater cleverness", "Greater size" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Jerry succeeds thanks to cleverness and agility.",
                    Category = "general",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 102,
                    Question = "Which everyday object is frequently used for exaggerated sound effects?",
                    Options = new List<string> { "Roller skates", "Typewriter", "Door slams and pans", "Telephones" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Pans, doors, and props punctuate the slapstick beats.",
                    Category = "general",
                    Difficulty = "medium"
                },
                // Additional five per category (characters, history, awards, general)
                // Characters (103-107)
                new QuizQuestion
                {
                    Id = 103,
                    Question = "Which cat sometimes appears as a rival house pet to Tom?",
                    Options = new List<string> { "Lightning", "Butch", "Meathead", "Topsy" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Lightning shows up as a speedy rival in some shorts.",
                    Category = "characters",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 104,
                    Question = "Which mouse is Jerry's baby nephew?",
                    Options = new List<string> { "Pip", "Nibbles/Tuffy", "Morty", "Mini" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Nibbles (Tuffy) appears as a diapered baby mouse.",
                    Category = "characters",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 105,
                    Question = "Which bulldog frequently disciplines Tom for disturbing his peace?",
                    Options = new List<string> { "Spike", "Rover", "Bruno", "Tyke" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Spike protects his territory and his son Tyke.",
                    Category = "characters",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 106,
                    Question = "Which cousin of Jerry is notably strong and muscular?",
                    Options = new List<string> { "Muscles", "Nibbles", "Morty", "Toodles" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Muscles helps Jerry when brute force is needed.",
                    Category = "characters",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 107,
                    Question = "Which glamorous feline does Tom often try to woo?",
                    Options = new List<string> { "Toodles Galore", "Felicia", "Snowball", "Sassy" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Toodles Galore is a recurring love interest for Tom.",
                    Category = "characters",
                    Difficulty = "medium"
                },
                // History (108-112)
                new QuizQuestion
                {
                    Id = 108,
                    Question = "What was the first released Tom and Jerry short?",
                    Options = new List<string> { "Puss Gets the Boot", "The Midnight Snack", "The Night Before Christmas", "Fraidy Cat" },
                    CorrectAnswerIndex = 0,
                    Explanation = "'Puss Gets the Boot' premiered in 1940.",
                    Category = "history",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 109,
                    Question = "Which composer scored many classic MGM Tom and Jerry shorts?",
                    Options = new List<string> { "Carl Stalling", "Scott Bradley", "Elmer Bernstein", "Alan Menken" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Scott Bradley's energetic scores are iconic to the series.",
                    Category = "history",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 110,
                    Question = "Which director helmed the early-60s Prague-made shorts?",
                    Options = new List<string> { "Chuck Jones", "Gene Deitch", "Tex Avery", "Bob Clampett" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Gene Deitch's team produced the Rembrandt Films entries in Prague.",
                    Category = "history",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 111,
                    Question = "Which company produced the Jones-era 1963–1967 series?",
                    Options = new List<string> { "Sib Tower 12/MGM Animation", "DePatie–Freleng", "Warner Bros. Cartoons", "Filmation" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Jones produced them at Sib Tower 12 (later MGM Animation/Visual Arts).",
                    Category = "history",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 112,
                    Question = "In which decade did the original MGM cartoon unit close?",
                    Options = new List<string> { "1940s", "1950s", "1960s", "1970s" },
                    CorrectAnswerIndex = 1,
                    Explanation = "MGM closed its animation unit in 1958.",
                    Category = "history",
                    Difficulty = "easy"
                },
                // Awards (113-117)
                new QuizQuestion
                {
                    Id = 113,
                    Question = "Which wartime-themed short earned Tom and Jerry an Oscar?",
                    Options = new List<string> { "The Yankee Doodle Mouse", "The Lonesome Mouse", "Zoot Cat", "Springtime for Thomas" },
                    CorrectAnswerIndex = 0,
                    Explanation = "'The Yankee Doodle Mouse' (1943) won the Academy Award.",
                    Category = "awards",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 114,
                    Question = "Which Oscar-winning short features a classical pianist cat?",
                    Options = new List<string> { "The Cat Concerto", "Professor Tom", "Cat Fishin'", "Mouse in Manhattan" },
                    CorrectAnswerIndex = 0,
                    Explanation = "'The Cat Concerto' is renowned for Tom's piano performance.",
                    Category = "awards",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 115,
                    Question = "Which short about Jerry inspiring a composer won an Oscar?",
                    Options = new List<string> { "Johann Mouse", "Mouse Trouble", "Cue Ball Cat", "Dr. Jekyll and Mr. Mouse" },
                    CorrectAnswerIndex = 0,
                    Explanation = "'Johann Mouse' (1952) depicts Jerry with Johann Strauss Jr.",
                    Category = "awards",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 116,
                    Question = "How many total Oscar nominations did Tom and Jerry receive?",
                    Options = new List<string> { "7", "10", "13", "15" },
                    CorrectAnswerIndex = 2,
                    Explanation = "They received 13 nominations and won 7.",
                    Category = "awards",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 117,
                    Question = "Which short featuring musketeers attire won an Academy Award?",
                    Options = new List<string> { "The Two Mouseketeers", "Touché, Pussy Cat!", "Tom and Cherie", "Pet Peeve" },
                    CorrectAnswerIndex = 0,
                    Explanation = "'The Two Mouseketeers' (1952) was an Oscar winner.",
                    Category = "awards",
                    Difficulty = "easy"
                },
                // General (118-122)
                new QuizQuestion
                {
                    Id = 118,
                    Question = "Which best characterizes the conflict in Tom and Jerry?",
                    Options = new List<string> { "Hero vs. villain", "Cat vs. mouse rivalry", "Detective vs. thief", "Parent vs. child" },
                    CorrectAnswerIndex = 1,
                    Explanation = "The timeless cat-and-mouse rivalry defines the show.",
                    Category = "general",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 119,
                    Question = "Which household pet besides Tom often complicates the chase?",
                    Options = new List<string> { "Parrot", "Dog", "Goldfish", "Turtle" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Spike the bulldog and his pup Tyke frequently interfere.",
                    Category = "general",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 120,
                    Question = "Which describes Jerry's usual strategy?",
                    Options = new List<string> { "Ambush and brute force", "Trickery and agility", "Negotiation and truce", "Hiding only" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Jerry outwits Tom using speed and clever traps.",
                    Category = "general",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 121,
                    Question = "Which prop is commonly used for comedic chases?",
                    Options = new List<string> { "Bicycles", "Skates and roller skates", "Scooters", "Surfboards" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Skates are classic chase enhancers in slapstick scenes.",
                    Category = "general",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 122,
                    Question = "Which best summarizes most episode endings?",
                    Options = new List<string> { "Tom wins permanently", "Jerry wins permanently", "Balance resets for another chase", "They move away" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Status quo returns, setting up the eternal chase.",
                    Category = "general",
                    Difficulty = "easy"
                },
                // Another five per category (characters, history, awards, general)
                // Characters (123-127)
                new QuizQuestion
                {
                    Id = 123,
                    Question = "Which alley cat is Tom's frequent rival and sometimes friend?",
                    Options = new List<string> { "Butch", "Sylvester", "Claude", "Tommy" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Butch often competes with Tom for food or affection.",
                    Category = "characters",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 124,
                    Question = "What is the name of Spike's son, often seen with a pacifier?",
                    Options = new List<string> { "Tyke", "Pup", "Junior", "Tiny" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Tyke appears alongside Spike in many shorts.",
                    Category = "characters",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 125,
                    Question = "Which mouse is Jerry's diapered sidekick in multiple episodes?",
                    Options = new List<string> { "Morty", "Nibbles/Tuffy", "Minnie", "Pip" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Nibbles (Tuffy) joins Jerry in several antics.",
                    Category = "characters",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 126,
                    Question = "Which muscular mouse relative of Jerry often scares Tom?",
                    Options = new List<string> { "Muscles", "Mighty", "Max", "Moose" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Muscles' strength turns the tables on Tom.",
                    Category = "characters",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 127,
                    Question = "Who is the glamorous white cat Tom frequently courts?",
                    Options = new List<string> { "Toodles Galore", "Princess", "Snowflake", "Bella" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Toodles Galore appears as Tom's love interest.",
                    Category = "characters",
                    Difficulty = "medium"
                },
                // History (128-132)
                new QuizQuestion
                {
                    Id = 128,
                    Question = "Which 1940 short first introduced the cat-and-mouse duo?",
                    Options = new List<string> { "Puss Gets the Boot", "Mouse Trouble", "The Night Before Christmas", "The Midnight Snack" },
                    CorrectAnswerIndex = 0,
                    Explanation = "'Puss Gets the Boot' marked their debut.",
                    Category = "history",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 129,
                    Question = "Who were the primary directors of the classic MGM era?",
                    Options = new List<string> { "Hanna & Barbera", "Jones & Maltese", "Avery & Freleng", "Clampett & Tashlin" },
                    CorrectAnswerIndex = 0,
                    Explanation = "William Hanna and Joseph Barbera directed most classic shorts.",
                    Category = "history",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 130,
                    Question = "Where were the early-60s Deitch-era shorts produced?",
                    Options = new List<string> { "Paris", "Prague", "London", "New York" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Rembrandt Films produced them in Prague.",
                    Category = "history",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 131,
                    Question = "Which unit produced the Chuck Jones-era revival in the 1960s?",
                    Options = new List<string> { "MGM Animation/Visual Arts", "Warner Bros. Cartoons", "UPA", "DePatie–Freleng" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Sib Tower 12 later became MGM Animation/Visual Arts for Jones' entries.",
                    Category = "history",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 132,
                    Question = "Which early short featured a Christmas theme for Tom and Jerry?",
                    Options = new List<string> { "The Night Before Christmas", "Polka-Dot Puss", "Nit-Witty Kitty", "Love That Pup" },
                    CorrectAnswerIndex = 0,
                    Explanation = "'The Night Before Christmas' (1941) is a notable early holiday short.",
                    Category = "history",
                    Difficulty = "easy"
                },
                // Awards (133-137)
                new QuizQuestion
                {
                    Id = 133,
                    Question = "Which Oscar-winning short prominently features a concert performance?",
                    Options = new List<string> { "The Cat Concerto", "Mouse Trouble", "Heavenly Puss", "Dr. Jekyll and Mr. Mouse" },
                    CorrectAnswerIndex = 0,
                    Explanation = "'The Cat Concerto' is celebrated for Tom's piano act.",
                    Category = "awards",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 134,
                    Question = "Which short set during wartime won an Academy Award?",
                    Options = new List<string> { "The Yankee Doodle Mouse", "Casanova Cat", "Jerry and the Lion", "Salt Water Tabby" },
                    CorrectAnswerIndex = 0,
                    Explanation = "'The Yankee Doodle Mouse' captured the wartime spirit and won.",
                    Category = "awards",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 135,
                    Question = "Which short about Thanksgiving dinner won an Oscar?",
                    Options = new List<string> { "The Little Orphan", "The Mouse Comes to Dinner", "Cat and Dupli-Cat", "Solid Serenade" },
                    CorrectAnswerIndex = 0,
                    Explanation = "'The Little Orphan' (1949) received the Academy Award.",
                    Category = "awards",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 136,
                    Question = "How many Academy Awards did Tom and Jerry win in total?",
                    Options = new List<string> { "5", "6", "7", "8" },
                    CorrectAnswerIndex = 2,
                    Explanation = "They won seven Oscars for Best Animated Short Film.",
                    Category = "awards",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 137,
                    Question = "Which short featuring musketeer costumes earned an Oscar?",
                    Options = new List<string> { "The Two Mouseketeers", "Touché, Pussy Cat!", "Tom and Cherie", "Touche, Tom!" },
                    CorrectAnswerIndex = 0,
                    Explanation = "'The Two Mouseketeers' (1952) was an Oscar winner.",
                    Category = "awards",
                    Difficulty = "medium"
                },
                // General (138-142)
                new QuizQuestion
                {
                    Id = 138,
                    Question = "Which best describes Tom and Jerry's typical runtime format?",
                    Options = new List<string> { "Feature films", "Short cartoons", "Mini-series arcs", "Full-length episodes" },
                    CorrectAnswerIndex = 1,
                    Explanation = "They are primarily short animated films.",
                    Category = "general",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 139,
                    Question = "What is a frequent cause of Tom's failure to catch Jerry?",
                    Options = new List<string> { "Lack of interest", "Overcomplicated traps backfiring", "Jerry's allies abandoning him", "No music" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Tom's elaborate schemes often defeat themselves.",
                    Category = "general",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 140,
                    Question = "Which supporting character is a bulldog that protects his territory?",
                    Options = new List<string> { "Butch", "Spike", "Tyke", "Droopy" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Spike commonly intervenes when Tom causes trouble.",
                    Category = "general",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 141,
                    Question = "Which best summarizes Jerry's advantage in conflicts?",
                    Options = new List<string> { "Strength", "Cleverness and agility", "Allies only", "Magic" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Jerry is small but clever and nimble.",
                    Category = "general",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 142,
                    Question = "Which household area commonly features slapstick mishaps?",
                    Options = new List<string> { "Kitchen", "Bathroom", "Attic", "Garage" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Kitchen utensils and appliances enable many gags.",
                    Category = "general",
                    Difficulty = "easy"
                }
            };

            return questions;
        }
    }
}
