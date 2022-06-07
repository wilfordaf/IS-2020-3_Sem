namespace Backups.Models
{
    public class Storage
    {
        private readonly string _name;
        private readonly Repository _repository;

        public Storage(string name, Repository repository)
        {
            _name = name;
            _repository = repository;
        }

        public string Path =>
            $"{(_repository.Path == string.Empty ? string.Empty : $"{_repository.Path}/")}{_name}.zip";
    }
}
