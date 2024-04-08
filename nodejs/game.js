const readline = require('readline');

let playerXname, playerOname;
let turn;
let board;

const Turn = {
    XTURN: 0,
    OTURN: 1
};

const Status = {
    XWIN: 0,
    OWIN: 1,
    DRAW: 2,
    ONGOING: 3
};

function setup() {
    console.log(`Welcome to TicTacGo!
    Our rules are simple:
        - Each square on the board is numbered from 1 to 9.
        - When it is your turn, you enter the number of the square you want to play at
        - That's it! You expected more! Silly! Haha.
        `);
    const rl = readline.createInterface({
        input: process.stdin,
        output: process.stdout
    });

    rl.question("Enter playerX name: ", function (name) {
        playerXname = name;
        rl.question("Enter PlayerO name: ", function (name) {
            playerOname = name;
            rl.close();
            turn = Turn.XTURN;
            board = ["1", "2", "3", "4", "5", "6", "7", "8", "9"];
            drawBoard();
            start();
        });
    });
}

function start() {
    const rl = readline.createInterface({
        input: process.stdin,
        output: process.stdout
    });

    rl.setPrompt(makePrompt(turn));
    rl.prompt();

    rl.on('line', function (move) {
        const num = parseInt(move);
        if (isNaN(num) || num < 1 || num > 9) {
            console.log("Invalid move!");
        } else {
            const index = num - 1;
            const err = makeMove(index);
            if (!err) {
                drawBoard();
                switchTurns();
            } else {
                console.log("Make a legal move!");
            }

            const gameStatus = checkGameStatus();
            switch (gameStatus) {
                case Status.XWIN:
                    console.log(`${playerXname} has won!`);
                    rl.close();
                    break;
                case Status.OWIN:
                    console.log(`${playerOname} has won!`);
                    rl.close();
                    break;
                case Status.DRAW:
                    console.log("It's a draw!");
                    rl.close();
                    break;
                case Status.ONGOING:
                    rl.setPrompt(makePrompt(turn));
                    rl.prompt();
                    break;
            }
        }
    });
}

function makePrompt(turn) {
    return `${getPlayer(turn)} >> `;
}

function switchTurns() {
    turn = (turn === Turn.XTURN) ? Turn.OTURN : Turn.XTURN;
}

function getPlayer(turn) {
    return (turn === Turn.XTURN) ? playerXname : playerOname;
}

function makeMove(move) {
    const moveChar = (turn === Turn.XTURN) ? "X" : "O";
    const legalMoves = findLegalMovesIndices();
    if (legalMoves.includes(move)) {
        board[move] = moveChar;
        return null;
    } else {
        console.log("Not a legal move!");
        return new Error("Skipped illegal move");
    }
}

function findLegalMovesIndices() {
    const legal = [];
    const regex = /[0-9]/;
    board.forEach((val, i) => {
        if (regex.test(val)) {
            legal.push(i);
        }
    });
    return legal;
}

function drawBoard() {
    const prettyBoard = `${board[0]} | ${board[1]} | ${board[2]}\n${board[3]} | ${board[4]} | ${board[5]}\n${board[6]} | ${board[7]} | ${board[8]}`;
    console.log(prettyBoard);
}

function checkWin(playerChar) {
    return (board[0] === playerChar && board[1] === playerChar && board[2] === playerChar) ||
           (board[3] === playerChar && board[4] === playerChar && board[5] === playerChar) ||
           (board[6] === playerChar && board[7] === playerChar && board[8] === playerChar) ||
           (board[0] === playerChar && board[3] === playerChar && board[6] === playerChar) ||
           (board[1] === playerChar && board[4] === playerChar && board[7] === playerChar) ||
           (board[2] === playerChar && board[5] === playerChar && board[8] === playerChar) ||
           (board[0] === playerChar && board[4] === playerChar && board[8] === playerChar) ||
           (board[2] === playerChar && board[4] === playerChar && board[6] === playerChar);
}

function checkDraw() {
    return findLegalMovesIndices().length === 0;
}

function checkGameStatus() {
    if (checkWin("X")) {
        return Status.XWIN;
    }
    if (checkWin("O")) {
        return Status.OWIN;
    }
    if (checkDraw()) {
        return Status.DRAW;
    }
    return Status.ONGOING;
}

setup();
