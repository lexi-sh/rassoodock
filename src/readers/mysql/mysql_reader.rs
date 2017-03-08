use mysql as my;
use std::env;
use chrono::NaiveDateTime;
use readers::database_reader::DatabaseReader;
use database::stored_procedure::StoredProcedure;

pub struct MySqlReader {}

impl DatabaseReader for MySqlReader {
    fn read_stored_procedures(&self, database_name: String) -> Vec<StoredProcedure> {
        let ip_address_opt = env::var(::MYSQL_IP_ENV_VAR_NAME).ok();
        let username_opt = env::var(::MYSQL_USERNAME_ENV_VAR_NAME).ok();
        let password_opt = env::var(::MYSQL_PASSWORD_ENV_VAR_NAME).ok();

        let mut builder = my::OptsBuilder::new();
        builder.ip_or_hostname(ip_address_opt)
            .db_name(Some(database_name))
            .user(username_opt)
            .pass(password_opt);

        let pool = my::Pool::new(my::Opts::from(builder)).unwrap(); // This will panic with the error from mysql

        let results: Vec<StoredProcedure> = pool.prep_exec("SHOW PROCEDURE STATUS", ()).unwrap()
            .map(|mut result| {
                let mut row = result.unwrap();
                
                let modified: NaiveDateTime = row.take("Modified").unwrap();
                let name: String = row.take("Name").unwrap();
                let db: String = row.take("Db").unwrap();
                let object_type: String = row.take("Type").unwrap();
                
                StoredProcedure {
                    stored_proc_sql_string: name,
                    last_modified_on: modified
                }
            }).collect();
            results
    }
}

struct MySqlProcedure {
    name: String,
    sql_mode: String,
    text: String,
    charset: String,
    collation_connection: String,
    database_collation: String,
    definer: String,
    security_type: String,
    last_modified_on: NaiveDateTime
}