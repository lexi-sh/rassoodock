#[macro_use]
extern crate mysql;
extern crate rassoodock;

use rassoodock::readers::mysql::mysql_reader::MySqlReader;
use rassoodock::readers::database_reader::DatabaseReader;
use mysql as my;
use std::env;

static MYSQL_IP_ENV_VAR_NAME: &'static str = "MYSQL_IP_ADDRESS";
static MYSQL_USERNAME_ENV_VAR_NAME: &'static str = "MYSQL_USERNAME";
static MYSQL_PASSWORD_ENV_VAR_NAME: &'static str = "MYSQL_PASSWORD";
static DATABASE: &'static str = "rassoodock_mysql_test";

fn db_init() {

    let ip_address_opt = env::var(MYSQL_IP_ENV_VAR_NAME).ok();
    let username_opt = env::var(MYSQL_USERNAME_ENV_VAR_NAME).ok();
    let password_opt = env::var(MYSQL_PASSWORD_ENV_VAR_NAME).ok();
    let mut db_builder = my::OptsBuilder::new();
    db_builder.ip_or_hostname(ip_address_opt)
        .db_name(Some("mysql"))
        .user(username_opt)
        .pass(password_opt);

    let db_pool = my::Pool::new(my::Opts::from(db_builder)).unwrap();
    db_pool.prep_exec(format!("DROP DATABASE IF EXISTS {}", DATABASE), ()).unwrap();
    db_pool.prep_exec(format!("CREATE DATABASE {}", DATABASE), ()).unwrap();
}

fn init() {
    db_init();
    let ip_address_opt = env::var(MYSQL_IP_ENV_VAR_NAME).ok();
    let username_opt = env::var(MYSQL_USERNAME_ENV_VAR_NAME).ok();
    let password_opt = env::var(MYSQL_PASSWORD_ENV_VAR_NAME).ok();

    let mut builder = my::OptsBuilder::new();
    builder.ip_or_hostname(ip_address_opt)
        .db_name(Some(DATABASE))
        .user(username_opt)
        .pass(password_opt);

    let pool = my::Pool::new(my::Opts::from(builder)).unwrap();
    pool.prep_exec("CREATE TABLE Persons (
                        ID int NOT NULL AUTO_INCREMENT,
                        LastName varchar(255) NOT NULL,
                        FirstName varchar(255),
                        Age int,
                        PRIMARY KEY (ID)
                    );", ()).unwrap();

    let mut conn = pool.get_conn().unwrap();
    conn.query("CREATE PROCEDURE get_persons() 
                BEGIN 
                    SELECT * FROM Persons;
                END;").unwrap();    
}

#[test]
fn test_mysql_reader() {
    init();
    let reader = MySqlReader{};
    let stored_procs = reader.read_stored_procedures("rassoodock_mysql_test".to_owned());
    assert_eq!(stored_procs.len(), 1);
    let ref get_tuts = stored_procs[0];
    assert_eq!(get_tuts.stored_proc_sql_string, "get_persons");
}


