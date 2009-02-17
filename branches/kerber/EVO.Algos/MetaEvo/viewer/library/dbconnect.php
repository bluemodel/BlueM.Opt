<?php

//Verbindung zur MySql Datenbank herstellen

 mysql_connect("localhost",    "metaevo","bluemopt") 
 or die    ("Keine Verbindung moeglich");    mysql_select_db("metaevo_db") 
 or die    ("Die Datenbank existiert nicht");   



?> 