
CREATE TABLE users (
    user_id int identity(1,1) PRIMARY KEY,
    username VARCHAR(50) not null,
    email VARCHAR(100) NOT NULL,
    password VARCHAR(255) NOT NULL,
    birthdate DATE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    role VARCHAR(50) DEFAULT 'user'
);


--
CREATE TABLE Teams (
    team_id INT identity(1,1) PRIMARY KEY,
    team_name VARCHAR(100) NOT NULL UNIQUE,
    foundation_year INT,
    country VARCHAR(50)
);

-- Mock data for Team Table
INSERT INTO
    teams (team_name, foundation_year, country)
VALUES
    ('Real Madrid', '1902', 'Spain'),
    ('Manchester United', '1878', 'England'),
    ('FC Barcelona', '1899', 'Spain'),
    ('Bayern Munich', '1900', 'Germany'),
    ('AC Milan', '1899', 'Italy');

CREATE TABLE players (
    player_id INT identity(1,1) PRIMARY KEY,
    player_name VARCHAR(100) NOT NULL UNIQUE,
    position VARCHAR(50),
    birthdate DATE,
    team_id INT,
    FOREIGN KEY (team_id) REFERENCES teams(team_id) ON DELETE
    SET NULL
);

INSERT INTO
    players (player_name, position, birthdate, team_id)
VALUES
    ('Cristiano Ronaldo', 'Forward', '1985-02-05', 1),
    ('Lionel Messi', 'Forward', '1987-06-24', 3),
    ('Neymar Jr.', 'Forward', '1992-02-05', 3),
    ('Robert Lewandowski', 'Striker', '1988-08-21', 4),
    ('Zlatan Ibrahimović', 'Striker', '1981-10-03', 2);

CREATE TABLE trainers (
    trainer_id INT identity(1,1) PRIMARY KEY,
    trainer_name VARCHAR(100) NOT NULL,
    coaching_license VARCHAR(50),
    team_id INT,
    FOREIGN KEY (team_id) REFERENCES teams(team_id) ON DELETE
    SET NULL
);

-- Mock data for Trainer Table
INSERT INTO
    trainers (trainer_name, coaching_license, team_id)
VALUES
    ('Zinedine Zidane', 'UEFA Pro', 1),
    ('Ole Gunnar Solskjær', 'UEFA A', 2),
    ('Ronald Koeman', 'UEFA Pro', 3),
    ('Hansi Flick', 'UEFA Pro', 4),
    ('Stefano Pioli', 'UEFA Pro', 5);
    
    