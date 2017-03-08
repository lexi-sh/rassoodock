use std::fmt;
use chrono::NaiveDateTime;

pub struct StoredProcedure {
    pub stored_proc_sql_string: String,
    pub last_modified_on: NaiveDateTime
}


impl fmt::Display for StoredProcedure {
    fn fmt(&self, fmt: &mut fmt::Formatter) -> fmt::Result {
        fmt.write_str(&self.stored_proc_sql_string);
        Ok(())
    }
}
