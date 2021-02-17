<?php
error_reporting(E_ERROR);

$login = new Login();

switch($_GET["action"]){
    case "registerUser":
        $r = $login->registerUser($_GET["userName"], $_GET["password"], $_GET["repassword"], $_GET["registerKey"]);
        break;
    case "accessAccount":
        $r = $login->accessAccount($_GET["userName"], $_GET["password"], $_GET["registerKey"]);
        break;
    case "generateRegisterKey":
        $r = $login->generateRegisterKey($_GET["adminPassword"]);
        break;
    case "isALLOWED":
        $r = $login->isALLOWED($_GET["userName"]);
        break;
	case "IP":
        $r = $login->IP($_GET["userName"], urlencode($_GET["IP"]));
        break;
	 case "hwid":
         $r = $login->hwid($_GET["userName"]);
        break;
	 case "sendhwid":
        $r = $login->sendhwid($_GET["userName"], urlencode($_GET["registerKey"]));
        break;
	 case "Banned":
        $r = $login->Banned($_GET["userName"], $_GET["registerKey"]);
        break;
     case "SelectAPI":
        $r = $login->SelectAPI($_GET["userName"], $_GET["registerKey"]);
        break;
	 case "GETIP":
         $r = $login->GETIP($_GET["userName"]);
        break;
    default:
        $r = "Lucid City";
}

echo $r;

class Login{
    private function query($sql, $arg, $fetch = false){
        require "connection.php";
        $q = $db->prepare($sql);
        $q->execute($arg);
        return $fetch ? $q->fetch(2) : $q;
    }

    private function bcrypt($password){
        return password_hash($password, PASSWORD_BCRYPT, ["cost" => 10]);
    }

    private function userExist($username){
        return $this->query("SELECT accountID FROM accounts WHERE userName COLLATE latin1_bin LIKE ?", array($username), true)["accountID"];
    }

    private function isBanned($username){
        return $this->query("SELECT isBanned FROM accounts WHERE accountID = ?", array($this->getAccountID($username)), true)["isBanned"];
    }

    private function getAccountID($username){
        return $this->query("SELECT accountID FROM accounts WHERE userName COLLATE latin1_bin LIKE ?", array($username), true)["accountID"];
    }

    public function registerUser($username, $password, $repassword, $registerKey){
        if(empty($username) ||empty($password) || empty($registerKey) || empty($repassword)) return "Missing parameters";
        if(strlen($username)>20 || strlen($username) < 3) return "Your username is too short";
        if(strlen($password) < 3) return "Your password is too short";   
        if($password != $repassword) return "Passwords do not match";        
        $this->query("INSERT INTO accounts(userName, password) VALUES (?, ?)", array($username, $this->bcrypt($password)));
        return header("Location: registered/index.php");
    }

    public function accessAccount($username, $password, $registerKey){ 
        if(empty($username) || empty($password) || empty($registerKey)) return "Missing parameters";
        if(!$this->userExist($username)) return "Invalid Username and or Password";
        if($this->isBanned($username)) return "You are banned from Lucid City Roleplay. To request an unban please head over to our forums https://www.lucidcityrp.com/";
        $pass = $this->query("SELECT password FROM accounts WHERE userName COLLATE latin1_bin LIKE ?", array($username), true);
        return password_verify($password, $pass["password"]) ? "LOGIN_GOOD:LOGGED_IN" : "Invalid Username and or Password.";
    }

    public function isALLOWED($username){
        if(empty($username)) return "Missing parameters";
        return $this->query("SELECT Status FROM accounts WHERE accountID  = ?", array($this->getAccountID($username)), true)["Status"];
    }
	
	   public function hwid($username){
        if(empty($username)) return "Missing parameters";
        $result = $this->query("SELECT HWID FROM accounts WHERE accountID  = ?", array($this->getAccountID($username)), true)["HWID"];
		echo ($result);
    }
	    public function sendhwid($username, $registerKey){
         if(empty($username) || empty($registerKey)) return "Missing parameters";
		 $this->query("UPDATE accounts SET HWID = ? WHERE accounts.userName = ?", array($registerKey, $username));
		 }
		 
		  public function Banned($username, $registerKey){
         if(empty($username) || empty($registerKey)) return "Missing parameters";
		 $this->query("UPDATE accounts SET isALLOWED = ? WHERE accounts.userName = ?", array($registerKey, $username));
		 }

		   public function SelectAPI($username, $registerKey){
         if(empty($username) || empty($registerKey)) return "Missing parameters";
		 $this->query("UPDATE accounts SET API = ? WHERE accounts.userName = ?", array($registerKey, $username));
		 }
	
	   public function IP($username, $IP){
         if(empty($username) || empty($IP)) return "Missing parameters";
		 $this->query("UPDATE accounts SET IP = ? WHERE accounts.userName = ?", array($IP, $username));
		 }
		 
		  public function GETIP($username){
        if(empty($username)) return "Missing parameters";
        $result = $this->query("SELECT IP FROM accounts WHERE accountID  = ?", array($this->getAccountID($username)), true)["IP"];
		echo ($result);
    }

    public function generateRegisterKey($adminpassword, $size = 10){
        if($adminpassword != "test") return "NOT_ENOUGH_PRIVILEGES";
        $exist=false;
        do{
            $alpha = "abcdefhijklmnopqrstuvwxyzABCDEFHIJKLMNOPQRSTUVWXYZ0123456789";
            $key = "";
            for($i = 0; $i<$size; $i++){
                $key .= $alpha[mt_rand(0, strlen($alpha) - 1)];
            }
            if($this->keyExist($key)) $exist = true;
        }while($exist);
        $this->query("INSERT INTO registrationKeys(registerKey) VALUES(?)", array($key));
        return $key;
    }

    private function keyExist($key){
        return $this->query("SELECT registerKey FROM registrationKeys WHERE registerKey COLLATE latin1_bin LIKE ? AND userName IS NULL", array($key), true)["registerKey"];
    }
    
    private function AssignKey($username, $key){
        if(!$this->keyExist($key)) return false;
        $this->query("UPDATE registrationKeys SET userName = ? WHERE registerKey COLLATE latin1_bin LIKE ?", array($username, $key));
        return true;
    }
}
					
