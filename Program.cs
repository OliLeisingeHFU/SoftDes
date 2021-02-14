using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TranslationMemory
{

    //Simp
    public class IDGenerator
    {
        private int langLastID;
        private int wordLastID;
        private int tranLastID;
        private int userLastID;

        private IDGenerator()
        {
            langLastID = 1;
            wordLastID = 1;
            tranLastID = 1;
            userLastID = 1;
        }

        private static IDGenerator _instance;

        public static IDGenerator GetInstance()
        {
            if (_instance == null)
                _instance = new IDGenerator();
            return _instance;
        }

        public int generateID(int type)
        {
            switch(type)
            {
                case 0:
                    return langLastID++;
                case 1:
                    return wordLastID++;
                case 2:
                    return tranLastID++;
                default:
                    return userLastID++;

            } 
        }
    }

    // Userclasses
    class User
    {
        public string username { get; set; }
        public string password { get; set; }
        public int addedWords { get; set; }
        public string userType { get; set; }

        public User()
        {
            userType = "user";
            username = userType + "_" + IDGenerator.GetInstance().generateID(3);
            password = "0000";
        }

        public User(string nickname, string pw)
        {
            username = nickname;
            password = pw;
            userType = "user";
        }

        public void searchWord()
        {

        }

        public void addWord (string addedWord)
        {

            addedWords++;
        }

        public int getAddedWords()
        {
            return addedWords;
        }

        public void showTotalWords()
        {

        }
    }

    class Translator : User
    {
        public string translationRights { get; set; }
        public int translatedWords { get; set; }

        public Translator(string nickname, string pw, string language) : base(nickname, pw)
        {
            translationRights = language;
            userType = "translator";
        }

        public void showMissingTranslations()
        {

        }

        public void translateWord(int wordID)
        {

        }

        public int getTranslatedWords()
        {
            return translatedWords;
        }
    }

    class Admin : User
    {
        public Admin(string nickname, string pw) : base (nickname, pw)
        {
            userType = "admin";
        }

        public Admin() : base()
        {
            userType = "admin";
        }

        public void addLanguage()
        {

        }

        public void addTranslatorRights(Translator user, string language)
        {

        }

        public void promoteUser(User toPromote)
        {

        }
    }

    //Language class
    class Language
    {
        public int id { get; set; }
        public string name { get; set; }

        public Language(string _name)
        {
            name = _name;
            id = IDGenerator.GetInstance().generateID(0);
        }
    }

    //Word classes
    class Word
    {
        public int id { get; set; }
        public string word { get; set; }

        public Word(string _word)
        {
            word = _word;
            id = IDGenerator.GetInstance().generateID(1);
        }
    }

    class TranslatedWord : Word
    {
        public int languageID { get; set; }
        public int translationOfID { get; set; }

        public TranslatedWord(string _word, int _lang, int _tran) : base(_word)
        {
            languageID = _lang;
            translationOfID = _tran;
        }
    }

    //Factories
    class LoginFactory
    {
        public List<User> users { get; set; }
        public List<Translator> translators { get; set; }
        public List<Admin> admins { get; set; }
        public User currentUser;

        public LoginFactory()
        {
            string readJSON = File.ReadAllText("UserJSON.json");
            users = JsonConvert.DeserializeObject<List<User>>(readJSON);

            readJSON = File.ReadAllText("TranJSON.json");
            translators = JsonConvert.DeserializeObject<List<Translator>>(readJSON);

            readJSON = File.ReadAllText("AdminJSON.json");
            admins = JsonConvert.DeserializeObject<List<Admin>>(readJSON);
        }

        public void login(Boolean loggedin)
        {
            if(!loggedin)
            {
                Console.WriteLine("Username:");
                string user = Console.ReadLine();
                Console.WriteLine("Passwort:");
                string pw = Console.ReadLine();
                if (users.First(item => item.username == user && item.password == pw) != null)
                {
                    currentUser = users.First(item => item.username == user && item.password == pw);
                }
                else if (translators.First(item => item.username == user && item.password == pw) != null)
                {
                    currentUser = translators.First(item => item.username == user && item.password == pw);
                }
                else if (admins.First(item => item.username == user && item.password == pw) != null)
                {
                    currentUser = admins.First(item => item.username == user && item.password == pw);
                }
                else
                {
                    Console.WriteLine("Dieser Benutzer existiert nicht.");
                }
            }
        }

        public void register()
        {

        }

        public Boolean validateUsername(string username)
        {

            return true;
        }

        public User createUser()
        {

            return new User();
        }

        public Admin createAdmin(Admin existingAdmin)
        {
            return new Admin();
        }

        public void saveUsers()
        {
            string json = JsonConvert.SerializeObject(users);
            File.WriteAllText("UserJSON.json", json);

            json = JsonConvert.SerializeObject(translators);
            File.WriteAllText("TranJSON.json", json);

            json = JsonConvert.SerializeObject(users);
            File.WriteAllText("AdminJSON.json", json);
        }
    }

    class WordFactory
    {
        public List<Language> languages { get; set; }
        public List<Word> words { get; set; }
        public List<TranslatedWord> translations { get; set; }

        public Word[] allWords()
        {
            return new Word[5];
        }

        public Word createWord(string _word)
        {
            return new Word(_word);
        }

        public TranslatedWord createTranslation(string _word, int _lang, int _tran)
        {
            return new TranslatedWord(_word, _lang, _tran);
        }

        public Boolean validateTranslationRights(Translator _user, int _langid)
        {

            return false;
        }

        public void saveLanguages()
        {
            string json = JsonConvert.SerializeObject(languages);
            File.WriteAllText("LangJSON.json", json);
        }

        public void saveWords()
        {
            string json = JsonConvert.SerializeObject(words);
            File.WriteAllText("WordJSON.json", json);
            json = JsonConvert.SerializeObject(translations);
            File.WriteAllText("WordTranJSON.json", json);
        }

        public void loadUsers()
        {
            string readJSON = File.ReadAllText("WordJSON.json");
            words = JsonConvert.DeserializeObject<List<Word>>(readJSON);

            readJSON = File.ReadAllText("WordTranJSON.json");
            translations = JsonConvert.DeserializeObject<List<TranslatedWord>>(readJSON);
        }
    }





    // Program
    class Program
    {
        private static Boolean loggedIn = false;
        private static LoginFactory logFac = new LoginFactory();
        static void Main(string[] args)
        {
            while (!loggedIn)
            {
                Console.WriteLine("Hallo! Bitte drücken Sie die 0, um sich einzuloggen oder die 1 um sich zu registrieren.");
                if (Console.ReadLine() == "0")
                {
                    logFac.login(loggedIn);
                }
            }
            while (loggedIn)
            {

            }
        }
    }
}
