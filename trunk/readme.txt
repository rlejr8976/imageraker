
readme for pub

-home
http://code.google.com/p/imageraker/


1. 실제 버전과의 차이점
버전체크 및 에러 전송용 url 삭제.
배포용 사이닝 및 셋업 프로젝트 삭제

테스트용 사이닝(pfx) 비번 
test123


2. post build event
os가 32비트냐 64비트냐에 따라 다름. 다음은 윈7 64비트에서 사용한 post build event임.
(개발용으로 vs10을 사용하지만 어셈블리 등록을 위해 아래와 같이 vs8이 필요.)

"C:\Program Files (x86)\Microsoft Visual Studio 8\SDK\v2.0\Bin\gacutil" /if "$(TargetDir)Interop.SHDocVw.dll"
"C:\Program Files (x86)\Microsoft Visual Studio 8\SDK\v2.0\Bin\gacutil" /if "$(TargetPath)"
"C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\regasm" /unregister "$(TargetPath)"
"C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\regasm" /register "$(TargetPath)"


3. 디버깅을 위한 ie경로 역시 64비트에서는 아래와 같이 사용한다.

C:\Program Files (x86)\Internet Explorer\iexplore.exe



