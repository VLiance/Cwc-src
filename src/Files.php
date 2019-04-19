<?php
$current_dir = ".";    

$dir = opendir($current_dir);        // Open the sucker

$files = array();
while ($files[] = readdir($dir));
rsort($files);
closedir($dir);

foreach ($files as $file) {
		
//	$fileAss = new SplFileInfo(file);
//	$ext  = $fileAss->getExtension();
$sExt =  pathinfo($file, PATHINFO_EXTENSION); // gz

      if ($file != "" && $file != "." && $file != ".." && $file != "error_log"  && $sExt != "php"  && $sExt != "txt"){
		echo "|" .rawurlencode($file) ."\n";
	  }
	  
	
}

?>