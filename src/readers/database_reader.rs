use database::stored_procedure::StoredProcedure;

pub trait DatabaseReader {
    fn read_stored_procedures(&self, database_name: String) -> Vec<StoredProcedure>;
}
