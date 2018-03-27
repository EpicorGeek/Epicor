case
  when (@Days%5) > -1*(DATEPART(weekday, GETDATE()) - 6)
  then  DATEADD(day, @Days+((@Days/5)+1)*2, GETDATE())

  else
  DATEADD(day, @Days+(@Days/5)*2, GETDATE())
end