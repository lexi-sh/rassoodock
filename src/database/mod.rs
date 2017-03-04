pub mod StoredProc;

use self::StoredProc::StoredProcedure;

pub struct Database {
    stored_procs: Vec<StoredProcedure>
}