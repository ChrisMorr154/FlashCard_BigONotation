CREATE TABLE FlashCards (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Question TEXT NOT NULL,
    CorrectAnswer TEXT NOT NULL,
    IncorrectAnswer1 TEXT NOT NULL,
    IncorrectAnswer2 TEXT NOT NULL,
    Hint TEXT NOT NULL
);

INSERT INTO FlashCards (Question, CorrectAnswer, IncorrectAnswer1, IncorrectAnswer2, Hint)
VALUES
("What is the time complexity of binary search?", "O(log n)", "O(n)", "O(1)", "Think about how the list is halved each step.");
