using System;
using TimeTracker.API;

public class Main
{
    public static void main(String[] args)
    {
        ApiTest api = new ApiTest();
        api.registerAsync("descamps.m91@hotmail.com","Matysse","Descamps","test1234");
        api.loginAsync("descamps.m91@hotmail.com","test1234");
    }
}