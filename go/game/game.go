package game

import (
	"errors"
	"fmt"
	"os"
	"regexp"
	"strconv"
)

type Turn int

const (
	XTURN Turn = iota
	OTURN Turn = iota
)

var playerXname, playerOname string
var turn Turn
var board []string

type Status int

const (
	XWIN    Status = iota
	OWIN    Status = iota
	DRAW    Status = iota
	ONGOING Status = iota
)

func Setup() {
	fmt.Println(`Welcome to TicTacToe!
Our rules are simple:
	- Each square on the board is numbered from 1 to 9.
	- When it is your turn, you enter the number of the square you want to play at
	- That's it! You expected more! Silly! Haha.
	`)
	fmt.Print("Enter playerX name: ")
	fmt.Scanln(&playerXname)
	fmt.Print("Enter PlayerO name: ")
	fmt.Scanln(&playerOname)
	// fmt.Println(`
	// 1 | 2 | 3
	// 4 | 5 | 6
	// 7 | 8 | 9`)
	turn = XTURN
	board = []string{"1", "2", "3", "4", "5", "6", "7", "8", "9"}
	drawBoard()
}

func Start() {
	for {
		fmt.Print(makePrompt(&turn))
		var move string
		fmt.Scanln(&move)
		num, err := strconv.Atoi(move)
		if err != nil {
			fmt.Println("Not a valid number!")
		} else if num < 1 || num > 9 {
			fmt.Println("Invalid move!")
		}

		num = num - 1
		err = makeMove(num)
		if err == nil {
			drawBoard()
			switchTurns(&turn)
		} else {
			fmt.Println("Make a legal move!")
		}
		switch CheckGameStatus() {
		case XWIN:
			fmt.Printf("%s has won!\n", playerXname)
			os.Exit(0)
		case OWIN:
			fmt.Printf("%s has won!\n", playerOname)
			os.Exit(0)
		case DRAW:
			fmt.Println("It's a draw!")
			os.Exit(0)
		}

	}
}

func getPlayer(turn Turn) string {
	if turn == XTURN {
		return playerXname
	} else {
		return playerOname
	}
}

func makePrompt(turn *Turn) string {
	return fmt.Sprintf("%s >> ", getPlayer(*turn))
}

func switchTurns(turn *Turn) {
	if *turn == XTURN {
		*turn = OTURN
	} else {
		*turn = XTURN
	}
}

func makeMove(move int) error {
	var moveChar string
	if turn == XTURN {
		moveChar = "X"
	} else {
		moveChar = "O"
	}
	legalMoves := findLegalMovesIndices()
	if isInside(legalMoves, move) == true {
		board[move] = moveChar
		return nil
	} else {
		fmt.Println("Not a legal move!")
		return errors.New("Skipped illegal move")

	}
}

func findLegalMovesIndices() []int {
	var legal []int
	pattern := `[0-9]`
	regex := regexp.MustCompile(pattern)
	for i, val := range board {
		if regex.MatchString(val) {
			legal = append(legal, i)
		}
	}
	return legal
}

func isInside(legal []int, move int) bool {
	for _, val := range legal {
		if val == move {
			return true
		}
	}
	return false
}

func drawBoard() {
	// defaultBoard := []int{1, 2, 3, 4, 5, 6, 7, 8, 9}
	prettyBoard := fmt.Sprintf("%s | %s | %s\n%s | %s | %s\n%s | %s | %s", board[0], board[1], board[2], board[3], board[4], board[5], board[6], board[7], board[8])
	fmt.Println(prettyBoard)
}

func checkWin(char string) bool {
	// hori wins
	if (board[0] == char && board[1] == char && board[2] == char) ||
		(board[3] == char && board[4] == char && board[5] == char) ||
		(board[6] == char && board[7] == char && board[8] == char) ||
		// vert wins
		(board[0] == char && board[3] == char && board[6] == char) ||
		(board[1] == char && board[4] == char && board[7] == char) ||
		(board[2] == char && board[5] == char && board[8] == char) ||
		// diag wins
		(board[0] == char && board[4] == char && board[8] == char) ||
		(board[2] == char && board[4] == char && board[6] == char) {
		return true
	}
	return false
}

func checkDraw() bool {
	if len(findLegalMovesIndices()) == 0 {
		return true
	}
	return false
}

func CheckGameStatus() Status {
	if checkWin("X") == true {
		return XWIN
	}
	if checkWin("O") == true {
		return OWIN
	}
	if checkDraw() == true {
		return DRAW
	}
	return ONGOING
}