Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses
Namespace Contensive.Addons.aoGallerifficGallery
    '
    Public Class galleryClass
        Inherits Contensive.BaseClasses.AddonBaseClass
        '
        Public Overrides Function Execute(ByVal CP As BaseClasses.CPBaseClass) As Object
            Dim s As String = ""    '   string
            Try
                Dim subS As String = "" '   sub string
                Dim inS As String = ""  '   inner string
                Dim aiS As String = ""  '   anchor image string
                Dim adS As String = ""  '   add string
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
                Dim image As String = ""
                Dim title As String = ""
                Dim description As String = ""
                Dim galleryID As Integer = CP.Utils.EncodeInteger(CP.Doc.Var("Galleriffic Gallery"))
                Dim cacheName As String = ""
                Dim initialized As Boolean = CP.Utils.EncodeBoolean(CP.Site.GetProperty("Addon Initialized - Galleriffic Gallery"))
                Dim imgPointer As Integer = 0
                Dim mydate As DateTime = DateTime.Now
                Dim isAuthoring As Boolean = CP.User.IsAuthoring("")
                '
                If Not initialized Then
                    For imgPointer = 1 To 5
                        cs.Open("Galleriffic Images", "name='Sample " & imgPointer & "'", , , "imageFileName")
                        If cs.OK() Then
                            cs.SetField("imageFileName", "aoGallerifficGallery/sample" & imgPointer & ".jpg")
                        End If
                        cs.Close()
                    Next
                    CP.Site.SetProperty("Addon Initialized - Galleriffic Gallery", "true")
                End If
                '
                If galleryID = 0 Then
                    galleryID = CP.Content.GetRecordID("Galleriffic Image Galleries", "Default")
                End If
                '
                cacheName = "Galleriffic Image Gallery - " & galleryID
                '
                If galleryID = 0 Then
                    If CP.User.IsAdmin() Then
                        s = CP.Html.div(CP.Html.div("<b>Administrator</b><br /><br />A Gallerffic Image Gallery was not set for this instance. To select a Gallery, turn on Advanced Edit and select a Galleriffic Gallery from the drop down on the Addon's Options toolbar.", , "ccHintWrapperContent"), , "ccHintWrapper").Replace("<div ", "<div style=""margin: 3px;"" ")
                    End If
                Else
                    If Not isAuthoring Then
                        s = CP.Cache.Read(cacheName)
                    End If
                    '
                    If s = "" Then
                        cs.Open("Galleriffic Images", "gallerifficImageGalleryID=" & galleryID, "sortOrder")
                        Do While cs.OK()
                            title = cs.GetText("name")
                            image = CP.Site.FilePath & cs.GetText("imageFileName")
                            description = cs.GetText("description")
                            '
                            aiS = ""
                            If isAuthoring Then
                                aiS = cs.GetEditLink()
                            End If
                            aiS &= "<a class=""thumb"" href=""" & image & """ title=""" & title & """><div style=""background: center no-repeat url(" & image & "); background-size: cover;""><img class=""thumbSpacer"" src=""/images/spacer.gif"" alt=""" & title & """ /></div></a>"
                            '
                            subS = CP.Html.div("<a target=""_blank"" href=""" & image & """>View Full Size</a>", , "download")
                            subS += CP.Html.div(title, , "image-title")
                            subS += CP.Html.div(description, , "image-desc")
                            '
                            subS = CP.Html.div(subS, , "caption")
                            inS += CP.Html.li(aiS & subS)
                            cs.GoNext()
                        Loop
                        cs.Close()
                        '
                        If isAuthoring Then
                            adS += CP.Content.GetAddLink("Galleriffic Images", "", False, True)
                        End If
                        '
                        subS = CP.Html.div("", , "gallerifficControls", "controls")
                        subS += CP.Html.div(CP.Html.div("", , "loader", "loading") & CP.Html.div("", , "slideshow", "slideshow"), , "slideshow-container")
                        subS += CP.Html.div("", , "caption-container", "caption")
                        '
                        s += CP.Html.div(subS, , "gallerifficContent", "gallery")
                        s += CP.Html.div(CP.Html.ul(inS, , "thumbs noscript") & adS, , "gallerifficNavigation", "thumbs")
                        s += "<div style=""clear: both;""></div>"
                        '
                        s = CP.Html.div(CP.Html.div(s, , , "gallerifficContainer"), , , "gallerifficPage")
                        '
                        If not isAuthoring Then
                            CP.Cache.Save(cacheName, s, "Galleriffic Images,Galleriffic Image Galleries")
                        End If
                    End If
                End If
                '
            Catch ex As Exception
                CP.Site.ErrorReport(ex.Message)
            End Try
            Return s
        End Function
    End Class
    '
End Namespace
