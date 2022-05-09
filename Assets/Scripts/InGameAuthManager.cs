using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using UnityEngine.UI;
using System;

public class InGameAuthManager : MonoBehaviour
{

    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;
    [Header("UI")]
    public GameObject loginUI;
    public GameObject registerUI;

    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text warningGameSaveText;
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text RegwarningText;
    void Awake()
    {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    //Function for the login button
    public void LoginButton()
    {
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }

    public void RegisterButton()
    {
        //Call the login coroutine passing the email and password
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    private IEnumerator Login(string _email, string _password)
    {
        Debug.Log(":::" + _email + "  " + _password);
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "B³¹d logowania!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Podaj email";
                    break;
                case AuthError.MissingPassword:
                    message = "Podaj has³o";
                    break;
                case AuthError.WrongPassword:
                    message = "B³êdne has³o";
                    break;
                case AuthError.InvalidEmail:
                    message = "B³êdny email";
                    break;
                case AuthError.UserNotFound:
                    message = "Konto nie istnieje";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            //User is now logged in
            //Now get the result
            User = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            warningLoginText.color = Color.green;
            warningLoginText.text = "Zalogowano pomyœlnie";
            yield return new WaitForSeconds(1);
            warningGameSaveText.color = Color.green;
            warningGameSaveText.text = "Gra zapisana";
            SaveGameToFirebase();
        }
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            //If the username field is blank show a warning
            RegwarningText.text = "Podaj nazwê u¿ytkownika";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //If the password does not match show a warning
            RegwarningText.text = "Has³a siê nie zgadzaj¹!";
        }
        else
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "B³¹d rejestracji!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Podaj email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Podaj has³o";
                        break;
                    case AuthError.WeakPassword:
                        message = "Za s³abe has³o";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email zajêty";
                        break;
                }
                RegwarningText.text = message;
            }
            else
            {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result;

                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        RegwarningText.text = "B³¹d nazwy u¿ytkownika!";
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        RegwarningText.color = Color.green;
                        RegwarningText.text = "Zarejestrowano pomyœlnie";
                        yield return new WaitForSeconds(1);
                        RegwarningText.text = "";
                        registerUI.SetActive(false);
                        loginUI.SetActive(true);
                    }
                }
            }
        }
    }

    public void SaveGameToFirebase()
    {
        StartCoroutine(UpdateXp(GameManager.instance.experience));
        StartCoroutine(UpdatePotions(GameManager.instance.potionsCount));
        StartCoroutine(UpdateMonety(GameManager.instance.pesos));
        StartCoroutine(UpdateMaxHp(GameManager.instance.MaxHP));
        StartCoroutine(UpdateCurrentHP(GameManager.instance.currHP));
        StartCoroutine(UpdateCurrentMap(GameManager.instance.currMap));
        StartCoroutine(UpdateWeaponLvl(GameManager.instance.weaponLevele));
    }


    private IEnumerator UpdateXp(int _xp)
    {
        //Set the currently logged in user xp
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("xp").SetValueAsync(_xp);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Xp is now updated
        }
    }

    private IEnumerator UpdatePotions(int _potionsCount)
    {
        //Set the currently logged in user xp
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("potions").SetValueAsync(_potionsCount);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Xp is now updated
        }
    }

    private IEnumerator UpdateMonety(int _pesos)
    {
        //Set the currently logged in user xp
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("monety").SetValueAsync(_pesos);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Xp is now updated
        }
    }

    private IEnumerator UpdateMaxHp(int _maxhp)
    {
        //Set the currently logged in user xp
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("maxHP").SetValueAsync(_maxhp);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Xp is now updated
        }
    }

    private IEnumerator UpdateCurrentHP(int _currenthp)
    {
        //Set the currently logged in user xp
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("currentHP").SetValueAsync(_currenthp);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Xp is now updated
        }
    }

    private IEnumerator UpdateCurrentMap(string _currentmap)
    {
        //Set the currently logged in user xp
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("currentMap").SetValueAsync(_currentmap);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Xp is now updated
        }
    }

    private IEnumerator UpdateWeaponLvl(int _weaponlvl)
    {
        //Set the currently logged in user xp
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("weaponLvl").SetValueAsync(_weaponlvl);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Xp is now updated
        }
    }
}
