import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/_models/User';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  registerForm: FormGroup;
  user: User;

  constructor(
              public fb: FormBuilder,
              private toastr: ToastrService,
              private router: Router,
              private authService: AuthService) { 

  }

  ngOnInit() {
    this.validation();
  }

  validation() {
    this.registerForm = this.fb.group({
      fullName: ['',Validators.required],
      email: ['',[ Validators.required, Validators.email ]],
      userName: ['',Validators.required],
      passwords: this.fb.group({
        password: ['',[Validators.required, Validators.minLength(4)]],
        confirmPassword: ['',Validators.required]
      }, { validator : this.compararSenhas })      
    });
  }

  compararSenhas(fg: FormGroup){
    const confirmSenhaCtrl = fg.get('confirmPassword');
    if(confirmSenhaCtrl.errors == null || 'mismatch' in confirmSenhaCtrl.errors){
      if(confirmSenhaCtrl.value != fg.get('password').value)
        confirmSenhaCtrl.setErrors({ mismatch: true });
    } else {
      confirmSenhaCtrl.setErrors(null);
    }
  }

  cadastrarUsuario(){
    if(this.registerForm.valid){
      this.user = Object.assign(
        {password: this.registerForm.get('passwords.password').value},
        this.registerForm.value
      );
      this.authService.register(this.user).subscribe(
        ()=> {
          console.log('1');
          this.router.navigate(['/user/login']);
          this.toastr.success('Cadastro realizado');
        },
        error => {
          const erro = error.error;
          erro.forEach(element => {            
            switch (element.code) {
              case 'DuplicateUserName':
                this.toastr.error('Já existe um usuário com o nome escolhido!');
                break;            
              default:
                this.toastr.error(`Erro no cadastro! Codigo: ${element.code}`);
                break;
            }
            
          });
        }
      );
    }
  }

}
