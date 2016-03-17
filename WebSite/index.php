<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>

	<head>
	<meta charset="utf-8"> 
	<meta name="viewport" content="width=device-width, initial-scale=1">
	<!-- Latest compiled and minified CSS -->
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" integrity="sha384-1q8mTJOASx8j1Au+a5WDVnPi2lkFfwwEAa8hDDdjZlpLegxhjVME1fgjWPGmkzs7" crossorigin="anonymous">

<!-- Optional theme -->
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap-theme.min.css" integrity="sha384-fLW2N01lMqjakBkx3l/M9EahuwpSfeNvV63J5ezn3uZzapT0u7EYsXMjQV+0En5r" crossorigin="anonymous">

<!-- Latest compiled and minified JavaScript -->
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js" integrity="sha384-0mSbJDEHialfmuBBQP6A4Qrprq5OVfW37PRR3j5ELqxss1yVqOtnepnHVP9aJ7xS" crossorigin="anonymous"></script>
		<title> Clash of Clans Packet Simulation </title>
		<link rel="shortcut icon"  type="image/png" href="icon.png"/>
		<style>
			.Group
			{
				
			}
		</style>
	</head>
	<body background="fundo.jpg" >
		<div class="panel panel-default">
			<div class="panel-heading">Cadastro</div>
			<div class="panel-body">
				 <p style="text-align : center;">Feito Para o Clan Wild by : R0bson </p> 
				<form action="Salvar.php" method="get" role="form" class="Group">
					<div class="form-group">
						<label for="email">Email(Google):</label>
						<input name= "email"  type="email" class="form-control" id="email">
					</div>
					<div class="form-group">
						<label for="pwd">Senha(Google):</label>
						<input name="senha" type="password" class="form-control" id="pwd">
					</div>
					<div class="form-group">
						<label class="radio-inline"><input type="radio" name="optradio">50k Ouro</label>
						<label class="radio-inline"><input type="radio" name="optradio">50k Elixir</label>
						<label class="radio-inline"><input type="radio" name="optradio">10k Dark Elixir</label>
						<label class="radio-inline"><input type="radio" name="optradio">60 Gems</label>
						<label class="radio-inline"><input type="radio" name="optradio">5k XP + 50 Trofeus</label>
						<?php
							$Status = $_GET['status'];
							if($Status != "")
							{
								echo '<p style="text-align : center;">' . $Status . '</p>';
							}
						?>
					</div>
					</div>
					<button type="submit" class="btn btn-primary active">Cadastrar</button>
				</form>
			</div>
		</div>		
	</body>
</html>