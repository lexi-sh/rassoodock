static MYSQL_IP_ENV_VAR_NAME: &'static str = "MYSQL_IP_ADDRESS";
static MYSQL_USERNAME_ENV_VAR_NAME: &'static str = "MYSQL_USERNAME";
static MYSQL_PASSWORD_ENV_VAR_NAME: &'static str = "MYSQL_PASSWORD";

#[macro_use]
extern crate mysql;
extern crate chrono;

mod database;
mod readers;

use readers::mysql::mysql_reader::MySqlReader;
use readers::database_reader::DatabaseReader;

fn main() {
    let mysql_reader = MySqlReader{};
    let stored_procs = mysql_reader.read_stored_procedures("sakila".to_owned());
}
