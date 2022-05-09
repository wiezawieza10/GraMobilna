using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class AuthManager : MonoBehaviour
{

    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;

    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    [Header("UI")]
    public GameObject loginUI;
    public GameObject MainMenuUI;
    public Text NewGameText;
    public Button LoadGameButton;
    public Button ReturnButton;
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

    public void Return()
    {
        MainMenuUI.SetActive(true);
        loginUI.SetActive(false);
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

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
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
            StartCoroutine(LoadUserData());
            warningLoginText.color = Color.green;
            warningLoginText.text = "Game Loaded from Database";
            yield return new WaitForSeconds(1);
            //SceneManager.LoadScene("Main", LoadSceneMode.Single);
            MainMenuUI.SetActive(true);
            loginUI.SetActive(false);
            NewGameText.text = "Continue";
            LoadGameButton.interactable = false;
        }
    }

    private IEnumerator LoadUserData()
    {
        LoadedVals.instance.wasGameLoaded = true;
        //Get the currently logged in user data
        var DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            LoadedVals.instance.LoadedExperience = int.Parse(snapshot.Child("xp").Value.ToString());
            LoadedVals.instance.LoadedPotionsCount = int.Parse(snapshot.Child("potions").Value.ToString());
            LoadedVals.instance.LoadedPesos = int.Parse(snapshot.Child("monety").Value.ToString());
            LoadedVals.instance.LoadedSavedMap = snapshot.Child("currentMap").Value.ToString();
            LoadedVals.instance.LoadedWeaponLevele = int.Parse(snapshot.Child("weaponLvl").Value.ToString());
            LoadedVals.instance.LoadedCurrHP = int.Parse(snapshot.Child("currentHP").Value.ToString());
            LoadedVals.instance.LoadedMaxHP = int.Parse(snapshot.Child("maxHP").Value.ToString());
        }
    }

}
