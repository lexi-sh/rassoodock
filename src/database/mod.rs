pub mod stored_procedure;

use self::stored_procedure::StoredProcedure;

pub struct Database {
    stored_procs: Vec<StoredProcedure>
}