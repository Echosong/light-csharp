#CoreSns


add-migration 1206

Update-Database -Verbose


# docker 

 链接sqlserver 文件
 
 sed -i  /etc/ssl/openssl.cnf
 
 修改配置里面  TLSv1.2  为 TLSv1
 
 
  update Universities set Logo =   REPLACE(  Logo, 'https://img.diebian.net/school/logo', 'http://qn.tongchenghy.cn/Universities')
  
update UniversityScores set MajorCount = (select COUNT(distinct(PlanMajorId)) from UniversityMajorScores where PlanSchoolId = UniversityScores.PlanSchoolId)
