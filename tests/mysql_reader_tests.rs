#[macro_use]
extern crate mysql;
extern crate rassoodock;

use rassoodock::readers::mysql::mysql_reader::MySqlReader;
use rassoodock::readers::database_reader::DatabaseReader;
use mysql as my;
use std::env;

fn init() {
    let ip_address_opt = env::var("MYSQL_IP_ADDRESS").ok();
    let username_opt = env::var("MYSQL_USERNAME").ok();
    let password_opt = env::var("MYSQL_PASSWORD").ok();


    let mut builder = my::OptsBuilder::new();
    builder.ip_or_hostname(ip_address_opt)
        .db_name(Some("mysql"))
        .user(username_opt)
        .pass(password_opt);

    let pool = my::Pool::new(my::Opts::from(builder)).unwrap();
    pool.prep_exec("DROP DATABASE IF EXISTS rassoodock_mysql_test", ());
    pool.prep_exec("CREATE DATABASE rassoodock_mysql_test", ()).unwrap();
    pool.prep_exec("CREATE TABLE tuts(
        tutorial_id INT NOT NULL AUTO_INCREMENT,
        tutorial_title VARCHAR(100) NOT NULL,
        tutorial_author VARCHAR(40) NOT NULL,
        submission_date DATE,
        PRIMARY KEY ( tutorial_id )", ()).unwrap();
    pool.prep_exec("DELIMITER //
                    CREATE PROCEDURE get_tuts() 
                    BEGIN 
                        SELECT * FROM tuts;
                    END//", ()).unwrap();
    
}

#[test]
fn test_mysql_reader() {
    init();
    let reader = MySqlReader{};
    let stored_procs = reader.read_stored_procedures("rassoodock_mysql_test".to_owned());
    assert_eq!(stored_procs.len(), 1);
    let ref get_tuts = stored_procs[0];
    assert_eq!(get_tuts.stored_proc_sql_string, "get_tuts");
}

